using spoclient.Extensions;

namespace spoclient.Models
{
    public class ServerInfo
    {
        public string Entry { get; set; }

        public string Server { get; set; }

        public string User { get; set; }

        public string Password { get; set; }

        public string Port { get; set; }

        public string? PrivateKey { get; set; }


        public ServerInfo()
        {
            Entry = string.Empty;
            Server = string.Empty;
            User = string.Empty;
            Password = string.Empty;
            Port = string.Empty;
        }


        public ServerInfo(string entry, string server, string user, string password, string port, string? privateKey = null)
        {
            this.Entry = entry;
            this.Server = server;
            this.User = user;
            this.Password = password;
            this.Port = port;
            this.PrivateKey = privateKey;
        }


        public SecureServerInfo ToSecure()
        {
            return new SecureServerInfo(
                this.Entry,
                this.Server,
                this.User,
                this.Password.ToSecureString(),
                this.Port,
                this.PrivateKey?.ToSecureString()
            );
        }
    }
}
