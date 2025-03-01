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
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace spoclient.Service
{
    public class SshService
    {
        public event EventHandler<SshStateChangedEventArgs>? StateChanged;

        public event EventHandler<SshOutputEventArgs>? Output;


        public Models.ServerInfo? ServerInfo { get; private set; }


        public Renci.SshNet.ConnectionInfo ConnectionInfo { get; private set; }


        private bool IsExecutingCommand { get; set; } = false;


        private SshClient? client;

        private StreamReader? reader;

        private StreamWriter? writer;


        public SshService(ServerInfo serverInfo)
        {
            var host = serverInfo.Server;
            if (!int.TryParse(serverInfo.Port, out int port))
            {
                throw new ArgumentOutOfRangeException(nameof(serverInfo));
            }
            var user = serverInfo.User;

            var pass = Marshal.PtrToStringUni(Marshal.SecureStringToGlobalAllocUnicode(serverInfo.Password));

            ConnectionInfo = new PasswordConnectionInfo(host, port, user, pass);
        }


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



        private async Task ReadSshOutput(Stream shellStream)
        {
            try
            {
                var buffer = new char[2048];
                int bytesRead;
                var result = new StringBuilder(4096);
                while (client!.IsConnected && (bytesRead = await reader!.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    var output = new string(buffer, 0, bytesRead);
                    output = CleanEscapeSequences(output);

                    output.Split(output.Contains(Environment.NewLine) ? '\n' : '\r').ForEach(async line =>
                    {
                        if (line.Length > 0)
                        {
                            if (!IsExecutingCommand && line.TrimEnd().EndsWith('$') || line.TrimEnd().EndsWith('#'))
                            {
                                IsExecutingCommand = true;
                                writer!.WriteLine("echo $?");
                                writer.Flush();
                            }

                            if (line.Contains("[sudo] password"))
                            {
                                await Task.Delay(200);

                                writer!.WriteLine("apJqUK57");
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


        private string CleanEscapeSequences(string input)
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

        private Color ParseAnsiColor(string ansiCode)
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


        public async Task<SshCommandResult> ExecuteCommandAsync(string commandText)
        {
            if (!client!.IsConnected)
            {
                return new SshCommandResult(commandText, string.Empty, -1);
            }

            IsExecutingCommand = false;

            writer.WriteLine(commandText);
            await writer.FlushAsync();

            return new SshCommandResult(commandText, string.Empty, -0);
        }


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
