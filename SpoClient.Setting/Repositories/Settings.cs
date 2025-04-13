using Dapper;
using Microsoft.Data.Sqlite;
using System.Collections.Specialized;

namespace SpoClient.Setting.Repositories
{
    /// <summary>
    ///     Application settings repository
    /// </summary>
    public class Settings
    {
        /// <summary>
        ///     SQLite connection
        /// </summary>
        public SqliteConnection connection;


        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="connection"></param>
        public Settings(SqliteConnection connection)
        {
            this.connection = connection;
        }


        /// <summary>
        ///     Add a new setting
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, string? value)
        {
            connection.Execute(@"
                INSERT INTO Settings (Key, Value)
                VALUES (@Key, @Value)
            ", new
            {
                Key = key,
                Value = value,
            });
        }


        /// <summary>
        ///     Add a new setting
        /// </summary>
        /// <param name="setting"></param>
        public void Add(Models.Setting setting)
        {
            this.Add(setting.Key, setting.Value);
        }


        /// <summary>
        ///     Update an existing setting
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Update(string key, string? value)
        {
            connection.Execute(@"
                UPDATE Settings
                SET Value = @Value
                WHERE Key = @Key
            ", new
            {
                Key = key,
                Value = value,
            });
        }


        /// <summary>
        ///     Delete a setting
        /// </summary>
        /// <param name="key"></param>
        public void Delete(string key)
        {
            connection.Execute(@"
                DELETE FROM Settings
                WHERE Key = @Key
            ", new
            {
                Key = key,
            });
        }


        /// <summary>
        ///     Get a setting by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Models.Setting? Get(string key)
        {
            return connection.QuerySingleOrDefault<Models.Setting>(@"
                SELECT Key, Value
                FROM Settings
                WHERE Key = @Key
            ", new
            {
                Key = key,
            });
        }


        /// <summary>
        ///     Get all settings
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Models.Setting> GetAll()
        {
            return connection.Query<Models.Setting>(@"
                SELECT Key, Value
                FROM Settings
            ");
        }


        /// <summary>
        ///     Get all settings as a StringDictionary
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
