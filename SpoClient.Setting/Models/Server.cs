using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpoClient.Setting.Models
{
    public class Server
    {
        public int Id { get; private set; }


        public string Name { get; set; } = string.Empty;


        public string Host { get; set; } = string.Empty;


        public string Port { get; set; } = string.Empty;


        public string User { get; set; } = string.Empty;


        public string? Password { get; set; }


        public string? PrivateKey { get; set; }


        public string? SudoPassword { get; set; }




        public Server()
        { }



        internal Server(int id, string name, string host, string port, string user, string? password, string? privateKey, string? sudoPassword)
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


        //public Server(string name, string host, string port, string user, string? password, string? privateKey, string? sudoPassword)
        //{
        //    Id = 0;
        //    Name = name;
        //    Host = host;
        //    Port = port;
        //    User = user;
        //    Password = password;
        //    PrivateKey = privateKey;
        //    SudoPassword = sudoPassword;
        //}


        public static readonly Server Empty = new(0, string.Empty, string.Empty, string.Empty, string.Empty, null, null, null);



        public SecureServer ToSecure()
        {
            return new SecureServer(this);
        }
    }
}
