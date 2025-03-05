using Dapper;
using spoclient.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace spoclient.Models
{
    public class ServerSettingsRepository
    {
        private string connectionString;
        private readonly SecureString password;


        public ServerSettingsRepository(string password)
        {
            var databasePath = Assembly.GetExecutingAssembly().Location + ".settings";

            connectionString = $"Data Source={databasePath};version=3;";

            this.password = (new SecureString()).FromString(password);

            InitializeDatabase();
        }


        private void InitializeDatabase()
        {
            using var connection = new SQLiteConnection(connectionString);
            connection.Open();

            var password = this.password.ToUnsecureString();
            //connection.Execute($"PRAGMA key = '{password}';");

            connection.Execute(@"
                CREATE TABLE IF NOT EXISTS Servers (
                    Entry TEXT PRIMARY KEY,
                    Server TEXT NOT NULL,
                    User TEXT NOT NULL,
                    Password TEXT,
                    Port TEXT NOT NULL,
                    PrivateKey TEXT
                )
            ");

            connection.Close();
        }


        public void AddServer(ServerInfo server)
        {
            using var connection = new SQLiteConnection(connectionString);
            connection.Open();

            var password = this.password.ToUnsecureString();
            //connection.Execute($"PRAGMA key = '{password}';");

            connection.Execute(@"
                INSERT INTO Servers (Entry, Server, User, Password, Port, PrivateKey)
                VALUES (@Entry, @Server, @User, @Password, @Port, @PrivateKey)
            ", server);

            connection.Close();
        }


        public void UpdateServer(ServerInfo server, ServerInfo from)
        {
            using var connection = new SQLiteConnection(connectionString);
            connection.Open();

            var password = this.password.ToUnsecureString();
            //connection.Execute($"PRAGMA key = '{password}';");

            var EntryFrom = from.Entry;
            var obj = new
            {
                server.Entry,
                server.Server,
                server.User,
                server.Password,
                server.Port,
                server.PrivateKey,
                EntryFrom
            };

            connection.Execute(@"
                UPDATE Servers
                SET Entry = @Entry, Server = @Server, User = @User, Password = @Password, Port = @Port, PrivateKey = @PrivateKey
                WHERE Entry = @EntryFrom
            ", obj);

            connection.Close();
        }


        public void DeleteServer(string entry)
        {
            using var connection = new SQLiteConnection(connectionString);
            connection.Open();

            var password = this.password.ToUnsecureString();
            //connection.Execute($"PRAGMA key = '{password}';");

            connection.Execute(@"
                DELETE FROM Servers
                WHERE Entry = @Entry
            ", new { Entry = entry });

            connection.Close();
        }


        public IEnumerable<SecureServerInfo> GetServers()
        {
            using var connection = new SQLiteConnection(connectionString);
            connection.Open();

            var password = this.password.ToUnsecureString();
            //connection.Execute($"PRAGMA key = '{password}';");

            var results = connection.Query<ServerInfo>(@"
                SELECT Entry, Server, User, Password, Port, PrivateKey
                FROM Servers
            ");

            foreach (var result in results)
            {
                yield return result.ToSecure();
            }

            connection.Close();
        }
    }
}
