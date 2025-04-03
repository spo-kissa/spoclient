using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpoClient.Setting
{
    public static class SqliteConnectionExtensions
    {
        /// <summary>
        ///     DBが暗号化されているかどうかを判定します
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool IsCipherEncrypted(this SqliteConnection connection)
        {
            ArgumentNullException.ThrowIfNull(connection);
            using var command = connection.CreateCommand();
            command.CommandText = "PRAGMA cipher_version;";
            var version = command.ExecuteScalar()?.ToString();

            return !string.IsNullOrEmpty(version);
        }


        /// <summary>
        ///     SQLiteを暗号化します
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="password"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Encrypt(this SqliteConnection connection, string password)
        {
            ArgumentNullException.ThrowIfNull(connection);
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            using var command = connection.CreateCommand();
            command.CommandText = "PRAGMA key = '" + Escape(password) + "';";
            command.ExecuteNonQuery();
        }


        /// <summary>
        ///     SQLiteの暗号化を解除します
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="oldPassword"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Decrypt(this SqliteConnection connection, string oldPassword)
        {
            ArgumentNullException.ThrowIfNull(connection);
            if (string.IsNullOrEmpty(oldPassword))
            {
                throw new ArgumentNullException(nameof(oldPassword));
            }
            connection.Execute("PRAGMA key = '" + Escape(oldPassword) + "';");
            connection.Execute("PRAGMA rekey = '';");
        }


        public static void ChangePassword(this SqliteConnection connection, string oldPassword, string newPassword)
        {
            ArgumentNullException.ThrowIfNull(connection);
            if (string.IsNullOrEmpty(oldPassword))
            {
                throw new ArgumentNullException(nameof(oldPassword));
            }
            if (string.IsNullOrEmpty(newPassword))
            {
                throw new ArgumentNullException(nameof(newPassword));
            }
            connection.Execute("PRAGMA key = '" + Escape(oldPassword) + "';");
            connection.Execute("PRAGMA rekey = '" + Escape(newPassword) + "';");
        }


        /// <summary>
        ///     SQLの文字列をエスケープします(超簡易版)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Escape(string value) => value.Replace("'", "''");


        /// <summary>
        ///     SQLを実行します
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sql"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Execute(this SqliteConnection connection, string sql)
        {
            ArgumentNullException.ThrowIfNull(connection);
            if (string.IsNullOrEmpty(sql))
            {
                throw new ArgumentNullException(nameof(sql));
            }
            using var command = connection.CreateCommand();
            command.CommandText = sql;
            command.ExecuteNonQuery();
        }
    }
}
