using Renci.SshNet;
using Renci.SshNet.Common;
using spoclient.Extensions;
using spoclient.Models;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VtNetCore.Avalonia;

namespace spoclient.Terminals
{
    public class SshConnection : IConnection, IDisposable
    {
        private const int BufferSize = 4096;


        private readonly ConnectionInfo connectionInfo;


        private SshClient? sshClient = null;


        private ShellStream? shellStream = null;


        private CancellationTokenSource? cancellationSource;


        private uint columns = 0, rows = 0, width = 0, height = 0;


        public ConnectionInfo ConnectionInfo => connectionInfo;


        public SshIdentification? Identification = null;


        public bool IsConnected => sshClient?.IsConnected ?? false;


        public event EventHandler<SshStateChangedEventArgs>? StateChanged;


        public event EventHandler<DataReceivedEventArgs>? DataReceived;


        public event EventHandler<EventArgs>? Closed;



        public SshConnection(ConnectionInfo connectionInfo)
        {
            this.connectionInfo = connectionInfo;
            this.Closed += OnClosed;
        }

        public SshConnection(SecureServerInfo server)
        {
            var host = server.Server;
            var port = int.TryParse(server.Port, out var number) ? number : 22;
            var user = server.User;
            var pass = server.Password;
            var pkey = server.PrivateKey;

            if (pkey is null)
            {
                this.connectionInfo = new PasswordConnectionInfo(host, port, user, pass.ToUnsecureBytes());
            }
            else
            {
                using var stream = new MemoryStream(pkey.ToUnsecureBytes(), false);
                stream.Position = 0;

                using var privateKey = new PrivateKeyFile(stream, pass.ToUnsecureString());
                this.connectionInfo = new PrivateKeyConnectionInfo(host, port, user, privateKey);
            }
        }


        public bool Connect()
        {
            StateChanged?.Invoke(this, new SshStateChangedEventArgs(SshConnectionState.Connecting));

            sshClient = new SshClient(connectionInfo);
            sshClient.ServerIdentificationReceived += OnServerIdentificationReceived;
            sshClient.Connect();

            if (!sshClient.IsConnected)
            {
                return false;
            }
            StateChanged?.Invoke(this, new SshStateChangedEventArgs(SshConnectionState.Connected));

            shellStream = sshClient.CreateShellStream("xterm", columns, rows, width, height, BufferSize);

            ReadData();

            return true;
        }

        public async Task<bool> ConnectAsync(CancellationToken cancellationToken)
        {
            StateChanged?.Invoke(this, new SshStateChangedEventArgs(SshConnectionState.Connecting));

            sshClient = new SshClient(connectionInfo);
            await sshClient.ConnectAsync(cancellationToken);

            if (!sshClient.IsConnected)
            {
                return false;
            }
            StateChanged?.Invoke(this, new SshStateChangedEventArgs(SshConnectionState.Connected));

            shellStream = sshClient.CreateShellStream("xterm", columns, rows, width, height, BufferSize);

            ReadData();

            return true;
        }


        private void OnServerIdentificationReceived(object? sender, SshIdentificationEventArgs e)
        {
            Identification = new SshIdentification(e);
            StateChanged?.Invoke(this, new SshStateChangedEventArgs(SshConnectionState.IdentificationReceived));
        }


        private void ReadData()
        {
            cancellationSource = new CancellationTokenSource();
            Task.Run(async () =>
            {
                var buffer = new byte[BufferSize];

                while (sshClient!.IsConnected && !cancellationSource.IsCancellationRequested)
                {
                    var bytesReceived = await shellStream!.ReadAsync(buffer, cancellationSource.Token);

                    if (bytesReceived > 0)
                    {
                        var receivedData = new byte[bytesReceived];
                        Buffer.BlockCopy(buffer, 0, receivedData, 0, bytesReceived);

                        DataReceived?.Invoke(this, new DataReceivedEventArgs { Data = receivedData });
                    }

                    await Task.Delay(5);
                }
            }, cancellationSource.Token);
        }


        public void SendData(byte[] data)
        {
            shellStream?.Write(data);
            shellStream?.Flush();
        }


        public void SendCommand(string command)
        {
            var bytes = Encoding.UTF8.GetBytes(command + "\n");

            SendData(bytes);
        }


        public void Disconnect()
        {
            Closed?.Invoke(this, EventArgs.Empty);

            cancellationSource?.Cancel();

            shellStream?.Dispose();
            shellStream = null;

            sshClient?.Disconnect();
        }


        public void SetTerminalWindowSize(int columns, int rows, int width, int height)
        {
            this.columns = (uint)columns;
            this.rows = (uint)rows;
            this.width = (uint)width;
            this.height = (uint)height;
        }

        private void OnClosed(object? sender, EventArgs e)
        {
            StateChanged?.Invoke(this, new SshStateChangedEventArgs(SshConnectionState.Disconnected));
        }


        public void Dispose()
        {
            Disconnect();

            shellStream?.Dispose();
            shellStream = null;

            sshClient?.Disconnect();
            sshClient?.Dispose();
            sshClient = null;

            if (connectionInfo is PasswordConnectionInfo passwordConnectionInfo)
            {
                passwordConnectionInfo.Dispose();
            }
            if (connectionInfo is PrivateKeyConnectionInfo privateKeyConnectionInfo)
            {
                privateKeyConnectionInfo.Dispose();
            }

            GC.SuppressFinalize(this);
        }
    }
}
