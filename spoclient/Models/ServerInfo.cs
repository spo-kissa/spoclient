using spoclient.Extensions;

namespace spoclient.Models
{
    /// <summary>
    ///     サーバー情報を保持するクラス
    /// </summary>
    public class ServerInfo
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
        public string Password { get; set; }

        /// <summary>
        ///     ポート番号
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        ///     秘密鍵
        /// </summary>
        public string? PrivateKey { get; set; }

        /// <summary>
        ///     sudoパスワード
        /// </summary>
        public string? SudoPassword { get; set; }


        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public ServerInfo()
        {
            Entry = string.Empty;
            Server = string.Empty;
            User = string.Empty;
            Password = string.Empty;
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
        public ServerInfo(string entry, string server, string user, string password, string port, string? privateKey = null, string? sudoPassword = null)
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
        ///     Secureな<see cref="SecureServerInfo">SecureServerInfo</see>に変換します
        /// </summary>
        /// <returns>SecureなSecureServerInfo</returns>
        public SecureServerInfo ToSecure()
        {
            return new SecureServerInfo(
                this.Entry,
                this.Server,
                this.User,
                this.Password.ToSecureString(),
                this.Port,
                this.PrivateKey?.ToSecureString(),
                this.SudoPassword?.ToSecureString()
            );
        }
    }
}
