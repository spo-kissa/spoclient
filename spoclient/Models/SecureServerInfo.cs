using spoclient.Extensions;
using System.Security;

namespace spoclient.Models
{
    public class SecureServerInfo
    {
        public string Entry { get; set; }

        public string Server { get; set; }

        public string User { get; set; }

        public SecureString Password { get; set; }

        public string Port { get; set; }

        public SecureString? PrivateKey { get; set; }



        public SecureServerInfo()
        {
            Entry = string.Empty;
            Server = string.Empty;
            User = string.Empty;
            Password = new SecureString();
            Port = string.Empty;
        }


        public SecureServerInfo(string entry, string server, string user, SecureString password, string port, SecureString? privateKey = null)
        {
            this.Entry = entry;
            this.Server = server;
            this.User = user;
            this.Password = password;
            this.Port = port;
            this.PrivateKey = privateKey;
        }


        public ServerInfo ToUnsecure()
        {
            return new ServerInfo(
                this.Entry,
                this.Server,
                this.User,
                this.Password.ToUnsecureString() ?? string.Empty,
                this.Port,
                this.PrivateKey?.ToUnsecureString()
            );
        }
    }
}
