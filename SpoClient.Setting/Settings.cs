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
        /// <summary>
        ///     シングルトンインスタンス
        /// </summary>
        private static Settings? instance = null;


        /// <summary>
        ///     設定ファイルが暗号化されている場合、パスワードを要求するイベント
        /// </summary>
        public event EventHandler? PasswordRequired;


        /// <summary>
        ///     シングルトンインスタンスを取得します
        /// </summary>
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


        /// <summary>
        ///     設定ファイルを開きます
        /// </summary>
        /// <param name="path"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<SqliteConnection?> OpenAsync(string path, string? password = null)
        {

            try
            {
                var encrypted = SQLite.IsEncrypted(path);
                if (encrypted && string.IsNullOrWhiteSpace(password))
                {
                    throw new ArgumentException("設定ファイルが暗号化されていますが、パスワードが指定されませんでした。", nameof(password));
                }

                var connectionString = new SqliteConnectionStringBuilder
                {
                    DataSource = path,
                }
                .ToString();

                var connection = new SqliteConnection(connectionString);
                await connection.OpenAsync();
                Connection = connection;
                return Connection;
            }
            catch (Exception ex)
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
