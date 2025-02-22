using Avalonia.Controls;
using Avalonia.Threading;
using Renci.SshNet;
using spoclient.Models;
using System;
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


        public TextBlock TextBlock { get; set; }


        private SshClient client;

        private StreamReader reader;

        private StreamWriter writer;


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

                    var shell = client.CreateShellStream("xterm", 80, 24, 800, 600, 1024);
                    reader = new StreamReader(shell, Encoding.UTF8);
                    writer = new StreamWriter(shell, Encoding.UTF8) { AutoFlush = true };

                    await Task.Run(() => ReadSshOutput(shell));
                }
                else
                {
                    StateChanged?.Invoke(this, new SshStateChangedEventArgs(SshState.FailToConnect));
                }

            }
            catch (Exception ex)
            {
                StateChanged?.Invoke(this, new SshStateChangedEventArgs(SshState.ErrorConnecting));
            }
        }


        private async void ReadSshOutput(Stream shellStream)
        {
            try
            {
                var buffer = new char[1024];
                int bytesRead;
                while (client.IsConnected && (bytesRead = await reader.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    var output = new string(buffer, 0, bytesRead);
                    output = CleanEscapeSequences(output);
                    Output?.Invoke(this, new SshOutputEventArgs(output));
                }
            }
            catch (Exception ex)
            {
                StateChanged?.Invoke(this, new SshStateChangedEventArgs(SshState.TerminalError));
            }
        }

        private string CleanEscapeSequences(string input)
        {
            input = Regex.Replace(input, "\x1B\\[\\?2004[hl]", "");
            input = Regex.Replace(input, "\x1B\\]0;[^\x07]*\x07", "");
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
            switch (ansiCode)
            {
                case "30": return Color.Black;
                case "31": return Color.Red;
                case "32": return Color.Green;
                case "33": return Color.Yellow;
                case "34": return Color.Blue;
                case "35": return Color.Magenta;
                case "36": return Color.Cyan;
                case "37": return Color.White;
                case "0": return Color.White; // Reset to default
                default: return Color.White;
            }
        }
    }
}
