using spoclient.Extensions;
using System.Security;

namespace spoclient.Models
{
    /// <summary>
    ///     サーバー情報を保持するクラス
    /// </summary>
    public class SecureServerInfo
    {
        /// <summary>
        ///     エントリ名
        /// </summary>
        public string Entry { get; set; }

        /// <summary>
        ///     サーバー名
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        ///     ユーザー名
        /// </summary>
        public string User { get; set; }

        /// <summary>
        ///     パスワード
        /// </summary>
        public SecureString Password { get; set; }

        /// <summary>
        ///    ポート番号
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        ///     秘密鍵
        /// </summary>
        public SecureString? PrivateKey { get; set; }

        /// <summary>
        ///     sudoパスワード
        /// </summary>
        public SecureString? SudoPassword { get; set; }



        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public SecureServerInfo()
        {
            Entry = string.Empty;
            Server = string.Empty;
            User = string.Empty;
            Password = new SecureString();
            Port = string.Empty;
        }


        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="entry">エントリ名</param>
        /// <param name="server">サーバー名</param>
        /// <param name="user">ユーザー名</param>
        /// <param name="password">パスワード</param>
        /// <param name="port">ポート番号</param>
        /// <param name="privateKey">秘密鍵</param>
        public SecureServerInfo(string entry, string server, string user, SecureString password, string port, SecureString? privateKey = null, SecureString? sudoPassword = null)
        {
            this.Entry = entry;
            this.Server = server;
            this.User = user;
            this.Password = password;
            this.Port = port;
            this.PrivateKey = privateKey;
            this.SudoPassword = sudoPassword;
        }


        /// <summary>
        ///    Unsecureな<see cref="ServerInfo">ServerInfo</see>に変換します
        /// </summary>
        /// <returns>UnsecureなServerInfo</returns>
        public ServerInfo ToUnsecure()
        {
            return new ServerInfo(
                this.Entry,
                this.Server,
                this.User,
                this.Password.ToUnsecureString() ?? string.Empty,
                this.Port,
                this.PrivateKey?.ToUnsecureString(),
                this.SudoPassword?.ToUnsecureString()
            );
        }
    }
}
