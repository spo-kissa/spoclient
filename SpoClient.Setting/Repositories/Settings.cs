using Dapper;
using Microsoft.Data.Sqlite;
using System.Collections.Specialized;

namespace SpoClient.Setting.Repositories
{
    public class Settings
    {
        public SqliteConnection Connection { get; private set; }


        public Settings(SqliteConnection connection)
        {
            Connection = connection;
        }


        public void Add(string key, string? value)
        {
            Connection.Execute(@"
                INSERT INTO Settings (Key, Value)
                VALUES (@Key, @Value)
            ", new
            {
                Key = key,
                Value = value,
            });
        }


        public void Add(Models.Setting setting)
        {
            this.Add(setting.Key, setting.Value);
        }



        public void Update(string key, string? value)
        {
            Connection.Execute(@"
                UPDATE Settings
                SET Value = @Value
                WHERE Key = @Key
            ", new
            {
                Key = key,
                Value = value,
            });
        }


        public void Delete(string key)
        {
            Connection.Execute(@"
                DELETE FROM Settings
                WHERE Key = @Key
            ", new
            {
                Key = key,
            });
        }


        /// <summary>
        ///     指定したキーの設定を取得します
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Models.Setting? Get(string key)
        {
            return Connection.QuerySingleOrDefault<Models.Setting>(@"
                SELECT Key, Value
                FROM Settings
                WHERE Key = @Key
            ", new
            {
                Key = key,
            });
        }


        /// <summary>
        ///     全ての設定を取得します
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Models.Setting> GetAll()
        {
            return Connection.Query<Models.Setting>(@"
                SELECT Key, Value
                FROM Settings
            ");
        }


        /// <summary>
        ///     全ての設定を取得します
        /// </summary>
        /// <returns></returns>
        public StringDictionary GetAllDictionary()
        {
            var settings = new StringDictionary();
            foreach (var setting in GetAll())
            {
                settings.Add(setting.Key, setting.Value);
            }

            return settings;
        }
    }
}
