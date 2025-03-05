using Avalonia.Controls;
using Avalonia.Threading;
using ImTools;
using Renci.SshNet;
using Renci.SshNet.Common;
using spoclient.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace spoclient.Service
{
    /// <summary>
    ///     SSH接続サービス
    /// </summary>
    public class SshService
    {
        /// <summary>
        ///     SSH接続状態が変更されたときに発生するイベント
        /// </summary>
        public event EventHandler<SshStateChangedEventArgs>? StateChanged;

        /// <summary>
        ///     SSH出力があったときに発生するイベント
        /// </summary>
        public event EventHandler<SshOutputEventArgs>? Output;


        /// <summary>
        ///     サーバー情報
        /// </summary>
        public SecureServerInfo? ServerInfo { get; private set; }


        /// <summary>
        ///     接続情報
        /// </summary>
        public ConnectionInfo ConnectionInfo { get; private set; }


        /// <summary>
        ///     最後のコマンドの終了コード
        /// </summary>
        public int LastExitCode { get; private set; } = -1;


        /// <summary>
        ///     SSHサービスの状態
        /// </summary>
        private SshServiceState state = SshServiceState.Idle;


        /// <summary>
        ///     終了コードプロバイダ
        /// </summary>
        private SshExitCodeProvider exitCodeProvider = new SshExitCodeProvider();


        /// <summary>
        ///     コマンド実行結果プロバイダ
        /// </summary>
        private readonly SshCommandResultProvider commandResultProvider = new();


        /// <summary>
        ///     SSHクライアント
        /// </summary>
        private SshClient? client;


        /// <summary>
        ///     SSHストリームリーダー
        /// </summary>
        private StreamReader? reader;


        /// <summary>
        ///     SSHストリームライター
        /// </summary>
        private StreamWriter? writer;


        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="serverInfo"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public SshService(SecureServerInfo serverInfo)
        {
            var host = serverInfo.Server;
            if (!int.TryParse(serverInfo.Port, out int port))
            {
                throw new ArgumentOutOfRangeException(nameof(serverInfo));
            }
            var user = serverInfo.User;

            var pass = Marshal.PtrToStringUni(Marshal.SecureStringToGlobalAllocUnicode(serverInfo.Password));

            ServerInfo = serverInfo;
            ConnectionInfo = new PasswordConnectionInfo(host, port, user, pass);
        }


        /// <summary>
        ///     SSHサーバーに接続します
        /// </summary>
        /// <returns></returns>
        public async Task ConnectAsync()
        {
            client = new SshClient(ConnectionInfo);

            try
            {
                StateChanged?.Invoke(this, new SshStateChangedEventArgs(SshState.Connecting));

                await Task.Run(() => client.Connect());

                if (client.IsConnected)
                {
                    StateChanged?.Invoke(this, new SshStateChangedEventArgs(SshState.Connected));

                    var terminalModes = new Dictionary<TerminalModes, uint>
                    {
                        { TerminalModes.ECHO, 53 },
                    };
                    var shell = client.CreateShellStream("xterm", 80, 24, 800, 600, 1024, terminalModes);
                    reader = new StreamReader(shell, Encoding.UTF8);
                    writer = new StreamWriter(shell, Encoding.UTF8) { AutoFlush = true, NewLine = "\x000A" };

                    await Task.Run(() => ReadSshOutput(shell));
                }
                else
                {
                    StateChanged?.Invoke(this, new SshStateChangedEventArgs(SshState.FailToConnect));
                }
            }
            catch (Exception ex)
            {
                StateChanged?.Invoke(this, new SshStateChangedEventArgs(SshState.ErrorConnecting, ex.Message));
            }
        }


        /// <summary>
        ///     SSHサーバーからの出力を読み取ります
        /// </summary>
        /// <param name="shellStream"></param>
        /// <returns></returns>
        private async Task ReadSshOutput(Stream shellStream)
        {
            try
            {
                var buffer = new char[2048];
                int bytesRead;
                var results = new List<string>(4096);
                var exitCodeLines = new List<string>();
                while (client!.IsConnected && (bytesRead = await reader!.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    var output = new string(buffer, 0, bytesRead);
                    output = CleanEscapeSequences(output);

                    output.Split(output.Contains(Environment.NewLine) ? '\n' : '\r').ForEach(async line =>
                    {
                        if (state == SshServiceState.GettingExitCode)
                        {
                            exitCodeLines.Add(line);
                        }
                        else
                        {
                            results.Add(line);
                        }

                        if (line.Length > 0)
                        {
                            // コマンド実行後にプロンプトを検出したら、終了コードを取得する
                            if (state == SshServiceState.RunningCommand && line.TrimEnd().EndsWith('$') || line.TrimEnd().EndsWith('#'))
                            {
                                state = SshServiceState.GettingExitCode;
                                exitCodeProvider.SetValue(LastExitCode, state);

                                // コマンド実行結果を通知する
                                commandResultProvider.SetResult(string.Join("\n", results)).SetState(state);

                                // 実行結果を格納するバッファをクリアする
                                results.Clear();

                                writer!.WriteLine("echo $?");
                                writer.Flush();
                            }

                            // 終了コードを取得する
                            if (exitCodeLines.Count > 2)
                            {
                                if (state == SshServiceState.GettingExitCode)
                                {
                                    var lastLine = exitCodeLines[^2];
                                    if (int.TryParse(lastLine.Trim(), out int exitCode))
                                    {
                                        state = SshServiceState.Idle;
                                        LastExitCode = exitCode;
                                        exitCodeProvider.SetValue(LastExitCode, state);

                                        // コマンド実行結果を通知する
                                        commandResultProvider.SetExitCode(exitCode).SetState(state);

                                        // 終了コードを格納するバッファをクリアする
                                        exitCodeLines.Clear();
                                    }
                                }
                            }

                            // sudo パスワードを入力する
                            if (line.Contains("[sudo] password"))
                            {
                                await Task.Delay(200);

                                var ptr = Marshal.SecureStringToGlobalAllocUnicode(ServerInfo!.Password);
                                try
                                {
                                    var password = Marshal.PtrToStringUni(ptr);
                                    writer!.WriteLine(password);
                                }
                                finally
                                {
                                    Marshal.FreeHGlobal(ptr);
                                }
                                writer.Flush();
                            }
                        }
                    });

                    Output?.Invoke(this, new SshOutputEventArgs(output));
                }
            }
            catch (Exception ex)
            {
                StateChanged?.Invoke(this, new SshStateChangedEventArgs(SshState.TerminalError, ex.Message));
            }
        }


        /// <summary>
        ///     エスケープシーケンスをクリアします
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string CleanEscapeSequences(string input)
        {
            input = Regex.Replace(input, "\x1B\\[\\?2004[hl]", "");
            input = Regex.Replace(input, "\x1B\\]0;[^\x07]*\x07", "");
            input = Regex.Replace(input, "\x1B\\[([0-9;]*)m", "");
            return input;
        }


        private void ApplyAnsiColor(string input)
        {
            Regex ansiRegex = new Regex("\x1B\\[([0-9;]*)m");
            MatchCollection matches = ansiRegex.Matches(input);

            int lastIndex = 0;
            Color currentColor = Color.White;

            foreach (Match match in matches)
            {
                string beforeCode = input.Substring(lastIndex, match.Index - lastIndex);
                //terminalOutput.SelectionColor = currentColor;
                //terminalOutput.AppendText(beforeCode);

                currentColor = ParseAnsiColor(match.Groups[1].Value);
                lastIndex = match.Index + match.Length;
            }

            //terminalOutput.SelectionColor = currentColor;
            //terminalOutput.AppendText(input.Substring(lastIndex));
        }


        /// <summary>
        ///     ANSIカラーコードを解析します
        /// </summary>
        /// <param name="ansiCode"></param>
        /// <returns></returns>
        private static Color ParseAnsiColor(string ansiCode)
        {
            return ansiCode switch
            {
                "30" => Color.Black,
                "31" => Color.Red,
                "32" => Color.Green,
                "33" => Color.Yellow,
                "34" => Color.Blue,
                "35" => Color.Magenta,
                "36" => Color.Cyan,
                "37" => Color.White,
                "0" => Color.White,// Reset to default
                _ => Color.White,
            };
        }


        /// <summary>
        ///     コマンドを実行しその結果を取得します
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public async Task<SshCommandResult> ExecuteCommandAsync(string commandText)
        {
            if (!client!.IsConnected)
            {
                return new SshCommandResult(commandText, string.Empty, -1);
            }

            state = SshServiceState.RunningCommand;
            exitCodeProvider.SetValue(LastExitCode, state);
            commandResultProvider.SetCommandText(commandText).SetState(state);

            writer!.WriteLine(commandText);
            writer.Flush();

            var cancellationTokenSource = new CancellationTokenSource();
            try
            {
                var result = await commandResultProvider.GetResultWhenCommandExitedAsync(cancellationTokenSource.Token);
                return result;
            }
            catch(OperationCanceledException)
            {
                return new SshCommandResult(commandText, string.Empty, -0);
            }
        }


        /// <summary>
        ///     コマンドを安全に実行しその結果を取得します
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<SshCommandResult> ExecuteCommandSafeAsync(string commandText, CancellationToken cancellationToken = default)
        {
            if (!client!.IsConnected)
            {
                return new SshCommandResult(commandText, string.Empty, -1);
            }

            var command = client.CreateCommand(commandText, Encoding.UTF8);
            await command.ExecuteAsync(cancellationToken);
            return new SshCommandResult(command);
        }
    }
}