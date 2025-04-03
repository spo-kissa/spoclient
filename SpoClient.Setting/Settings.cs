using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace SpoClient.Setting
{
    public class Settings
    {
        private static Settings? instance = null;


        public event EventHandler? PasswordRequired;


        public static Settings Instance
        {
            get
            {
                instance ??= new Settings();
                return instance;
            }
        }


        public SecureString? Password { get; set; } = null;


        public SqliteConnection? Connection { get; private set; } = null;



        /// <summary>
        ///     コンストラクタ
        /// </summary>
        private Settings()
        {
        }



        public async Task<SqliteConnection?> OpenAsync(string path, string? password = null)
        {
            var encrypted = SQLite.IsEncrypted(path);
            if (encrypted && string.IsNullOrWhiteSpace(password))
            {
                //throw new ArgumentException("設定ファイルが暗号化されていますが、パスワードが指定されませんでした。", nameof(password));
            }

            try
            {
                var connectionString = new SqliteConnectionStringBuilder
                {
                    DataSource = path,
                }
                .ToString();

                var connection = new SqliteConnection(connectionString);
                await connection.OpenAsync();
                if (connection.IsCipherEncrypted())
                {
                    throw new InvalidOperationException("SQLiteの暗号化が解除されていません。");
                }
                Connection = connection;
                return Connection;
            }
            catch (Exception)
            {
                if (string.IsNullOrEmpty(password))
                {
                    PasswordRequired?.Invoke(this, EventArgs.Empty);
                    password = Password?.ToUnsecureString();
                }

                var connectionString = new SqliteConnectionStringBuilder
                {
                    DataSource = path,
                    Password = password,
                }
                .ToString();

                if (!string.IsNullOrEmpty(password))
                {
                    var connection = new SqliteConnection(connectionString);
                    await connection.OpenAsync();

                    Connection = connection;
                    return Connection;
                }
            }

            return null;
        }
    }
}
