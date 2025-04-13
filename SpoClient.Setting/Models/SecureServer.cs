using System.Security;

namespace SpoClient.Setting.Models
{
    public class SecureServer
    {
        public int Id { get; private set; }


        public string Name { get; set; } = string.Empty;


        public string Host { get; set; } = string.Empty;


        public string Port { get; set; } = string.Empty;


        public string User { get; set; } = string.Empty;


        public SecureString? Password { get; set; }



        public SecureString? PrivateKey { get; set; }


        public SecureString? SudoPassword { get; set; }



        public SecureServer(Server server)
        {
            Id = server.Id;
            Name = server.Name;
            Host = server.Host;
            Port = server.Port;
            User = server.User;
            Password = server.Password?.ToSecureString();
            PrivateKey = server.PrivateKey?.ToSecureString();
            SudoPassword = server.SudoPassword?.ToSecureString();
        }


        public SecureServer(int id, string name, string host, string port, string user, SecureString? password, SecureString? privateKey, SecureString? sudoPassword)
        {
            Id = id;
            Name = name;
            Host = host;
            Port = port;
            User = user;
            Password = password;
            PrivateKey = privateKey;
            SudoPassword = sudoPassword;
        }


        public static readonly SecureServer Empty = new(0, string.Empty, string.Empty, string.Empty, string.Empty, null, null, null);
    }
}
