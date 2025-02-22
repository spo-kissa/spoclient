using Renci.SshNet;
using Renci.SshNet.Common;
using System.Text;
using System.Text.RegularExpressions;

namespace WinFormsTest
{
    public partial class SshTerminal : Form
    {
        private SshClient client;
        private StreamReader reader;
        private StreamWriter writer;


        public SshTerminal()
        {
            InitializeComponent();
        }


        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            sendButton.Click += SendCommand;

            ConnectToSsh();
        }


        private void ConnectToSsh()
        {
            string host = "172.21.46.241";
            string username = "daisuke";
            string password = "apJqUK57";

            client = new SshClient(host, 22, username, password);
            try
            {
                client.Connect();
                if (client.IsConnected)
                {
                    var shell = client.CreateShellStream("xterm", 80, 24, 800, 600, 1024);
                    reader = new StreamReader(shell, Encoding.UTF8);
                    writer = new StreamWriter(shell, Encoding.UTF8) { AutoFlush = true };

                    Task.Run(() => ReadSshOutput(shell));
                }
            }
            catch (Exception ex)
            {
                this.BeginInvoke((MethodInvoker)delegate { terminalOutput.AppendText("Error: " + ex.Message + "\n"); });
            }
        }

        private async void ReadSshOutput(Stream shellStream)
        {
            try
            {
                char[] buffer = new char[1024];
                int bytesRead;
                while (client.IsConnected && (bytesRead = await reader.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    string output = new string(buffer, 0, bytesRead);
                    output = CleanEscapeSequences(output); // 制御シーケンスを除去
                    this.BeginInvoke((MethodInvoker)delegate { ApplyAnsiColor(output); });
                }
            }
            catch (Exception ex)
            {
                this.BeginInvoke((MethodInvoker)delegate { terminalOutput.AppendText("Error: " + ex.Message + "\n"); });
            }
        }

        private string CleanEscapeSequences(string input)
        {
            // Bracketed Paste Mode, ウィンドウタイトル変更などの制御シーケンスを除去
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
                terminalOutput.SelectionColor = currentColor;
                terminalOutput.AppendText(beforeCode);

                currentColor = ParseAnsiColor(match.Groups[1].Value);
                lastIndex = match.Index + match.Length;
            }

            terminalOutput.SelectionColor = currentColor;
            terminalOutput.AppendText(input.Substring(lastIndex));
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


        private void SendCommand(object? sender, EventArgs e)
        {
            if (client.IsConnected)
            {
                string command = inputBox.Text;
                writer.Write(command + (char)0x000A);
                inputBox.Clear();
            }
        }
    }
}
