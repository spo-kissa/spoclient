using System.Security;

namespace spoclient.Models
{
    public class ServerInfo
    {
        public string Entry { get; set; }

        public string Server { get; set; }

        public string User { get; set; }

        public SecureString Password { get; set; }

        public string Port { get; set; }


        public ServerInfo(string entry, string server, string user, SecureString password, string port)
        {
            this.Entry = entry;
            this.Server = server;
            this.User = user;
            this.Password = password;
            this.Port = port;
        }
    }
}
