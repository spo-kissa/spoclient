using Microsoft.Data.Sqlite;
using System.Text;

namespace SpoClient.Setting
{
    public class SQLite
    {
        /// <summary>
        ///     SQLiteのヘッダーから暗号化されているかどうかを判定します
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsEncrypted(string path)
        {
            if (!File.Exists(path))
            {
                return false;
            }
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            byte[] header = new byte[16];
            fs.Read(header, 0, 16);
            string headerText = Encoding.ASCII.GetString(header);
            return headerText != "SQLite format 3\0";
        }


        /// <summary>
        ///     SQLiteのバージョンを取得します
        /// </summary>
        /// <returns></returns>
        public static string GetVersion()
        {
            using var conn = new SqliteConnection("Data Source=:memory:");
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT sqlite_version();";
            var version = cmd.ExecuteScalar()?.ToString();
            return $"{version}";
        }


        /// <summary>
        ///     SQLCipherのバージョンを取得します
        /// </summary>
        /// <returns></returns>
        public static string GetCipherVersion()
        {
            using var conn = new SqliteConnection("Data Source=:memory:");
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "PRAGMA cipher_version;";
            var version = cmd.ExecuteScalar()?.ToString();
            return $"{version}";
        }
    }
}
