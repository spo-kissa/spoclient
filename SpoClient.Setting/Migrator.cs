using Microsoft.Data.Sqlite;
using SQLitePCL;

namespace SpoClient.Setting
{
    public class Migrator
    {
        private readonly string dbPath;


        private readonly SqliteConnection connection;


        public Migrator(string dbPath)
        {
            this.dbPath = dbPath;

            Batteries_V2.Init();

            var connectionString = new SqliteConnectionStringBuilder
            {
                DataSource = dbPath,
                Mode = SqliteOpenMode.ReadWriteCreate,
            }
            .ToString();

            connection = new SqliteConnection(connectionString);
            Task.Run(async () =>
            {
                await connection.OpenAsync();
            })
            .Wait();
        }


        public Migrator(SqliteConnection connection)
        {
            dbPath = connection.DataSource;

            Batteries_V2.Init();

            this.connection = connection;
        }


        public async Task ApplyMigrationsFromResourcesAsync(string resourceNamespace)
        {
            CreateSchemaMigrationsTableIfNeeded(connection);
            var applied = GetAppliedMigrations(connection);
            var migrations = SqlResourceLoader.LoadSqlMigrations(resourceNamespace).ToList();

            foreach (var m in migrations.OrderBy(m => m.Name))
            {
                if (applied.Contains(m.Name)) { continue; }

                if (!string.IsNullOrEmpty(m.DependsOn) && !applied.Contains(m.DependsOn))
                {
                    System.Diagnostics.Debug.WriteLine($"Skipping {m.Name} (depends on {m.DependsOn})");
                    continue;
                }

                System.Diagnostics.Debug.WriteLine($"Applying migration: {m.Name} - {m.Description}");

                using var tx = connection.BeginTransaction();
                using var cmd = connection.CreateCommand();
                
                cmd.Transaction = tx;
                cmd.CommandText = m.Sql;
                await cmd.ExecuteNonQueryAsync();

                cmd.CommandText = "INSERT INTO schema_migrations (filename, applied_at) VALUES (@name, CURRENT_TIMESTAMP);";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@name", m.Name);
                await cmd.ExecuteNonQueryAsync();

                tx.Commit();
            }
        }


        private static void CreateSchemaMigrationsTableIfNeeded(SqliteConnection connection)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS schema_migrations (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    filename TEXT NOT NULL UNIQUE,
                    applied_at DATETIME NOT NULL
                )
            ";
            cmd.ExecuteNonQuery();
        }


        private static HashSet<string> GetAppliedMigrations(SqliteConnection connection)
        {
            var applied = new HashSet<string>();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT filename FROM schema_migrations;";
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                applied.Add(reader.GetString(0));
            }
            return applied;
        }
    }
}
