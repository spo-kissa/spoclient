using Dapper;
using Microsoft.Data.Sqlite;
using SpoClient.Setting.Models;

namespace SpoClient.Setting.Repositories
{
    /// <summary>
    ///     サーバー情報を管理するリポジトリ
    /// </summary>
    public class Servers
    {
        /// <summary>
        ///     データベース接続オブジェクト
        /// </summary>
        public SqliteConnection Connection { get; private set; }


        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="connection"></param>
        public Servers(SqliteConnection connection)
        {
            Connection = connection;
        }


        /// <summary>
        ///     サーバー情報を追加します
        /// </summary>
        /// <param name="server"></param>
        public void Add(Server server)
        {
            Connection.Execute(@"
                INSERT INTO Servers (Name, Host, User, Password, Port, PrivateKey, SudoPassword)
                VALUES (@Name, @Host, @User, @Password, @Port, @PrivateKey, @SudoPassword)
            ", new
            {
                server.Name,
                server.Host,
                server.User,
                server.Password,
                server.Port,
                server.PrivateKey,
                server.SudoPassword,
            });
        }


        /// <summary>
        ///     セキュアなサーバー情報を追加します
        /// </summary>
        /// <param name="secureServer"></param>
        public void Add(SecureServer secureServer)
        {
            this.Add(new Server(secureServer.Id, secureServer.Name, secureServer.Host, secureServer.Port, secureServer.User, secureServer.Password?.ToUnsecureString(), secureServer.PrivateKey?.ToUnsecureString(), secureServer.SudoPassword?.ToUnsecureString()));
        }


        /// <summary>
        ///     サーバー情報を更新します
        /// </summary>
        /// <param name="server"></param>
        public void Update(Server server)
        {
            Connection.Execute(@"
                UPDATE Servers
                SET Name = @Name, Host = @Host, User = @User, Password = @Password, Port = @Port, PrivateKey = @PrivateKey, SudoPassword = @SudoPassword
                WHERE Id = @Id
            ", new
            {
                server.Name,
                server.Host,
                server.User,
                server.Password,
                server.Port,
                server.PrivateKey,
                server.SudoPassword,
                server.Id
            });
        }


        /// <summary>
        ///     セキュアなサーバー情報を更新します
        /// </summary>
        /// <param name="secureServer"></param>
        public void Update(SecureServer secureServer)
        {
            this.Update(new Server(secureServer.Id, secureServer.Name, secureServer.Host, secureServer.Port, secureServer.User, secureServer.Password?.ToUnsecureString(), secureServer.PrivateKey?.ToUnsecureString(), secureServer.SudoPassword?.ToUnsecureString()));
        }


        /// <summary>
        ///     サーバー情報を削除します
        /// </summary>
        /// <param name="id"></param>
        public void Delete(long id)
        {
            Connection.Execute(@"
                DELETE FROM Servers
                WHERE Id = @Id
            ", new { Id = id });
        }


        /// <summary>
        ///     サーバー情報を削除します
        /// </summary>
        /// <param name="server"></param>
        public void Delete(Server server)
        {
            this.Delete(server.Id);
        }


        /// <summary>
        ///     セキュアなサーバー情報を削除します
        /// </summary>
        /// <param name="secureServer"></param>
        public void Delete(SecureServer secureServer)
        {
            this.Delete(secureServer.Id);
        }


        /// <summary>
        ///     サーバー情報一覧を取得します
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Server> GetAll()
        {
            return Connection.Query<Server>(@"
                SELECT Id, Name, Host, User, Password, Port, PrivateKey, SudoPassword
                FROM Servers
            ");
        }


        /// <summary>
        ///     セキュアなサーバー情報一覧を取得します
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SecureServer> GetSecureAll()
        {
            return GetAll().Select(x => new SecureServer(x));
        }
    }
}
