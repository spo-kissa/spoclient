using System.Reflection;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SpoClient.Setting
{
    internal static class SqlResourceLoader
    {
        public static IEnumerable<SqlMigration> LoadSqlMigrations(string ns)
        {
            var asm = Assembly.GetExecutingAssembly();
            var resources = asm.GetManifestResourceNames()
                .Where(r => r.StartsWith(ns) && r.EndsWith(".sql"))
                .ToList();

            foreach (var resource in resources.OrderBy(r => r))
            {
                using var stream = asm.GetManifestResourceStream(resource);
                if (stream is null)
                {
                    break;
                }
                using var reader = new StreamReader(stream);
                var text = reader.ReadToEnd();

                var migration = ParseMigrationWithYaml(text);
                if (string.IsNullOrEmpty(migration.Name))
                {
                    throw new Exception($"マイグレーションに'name'がありません: {resource}");
                }

                yield return migration;
            }
        }


        private static SqlMigration ParseMigrationWithYaml(string rawText)
        {
            var lines = rawText.Split('\n');
            if (!lines[0].Trim().StartsWith("---"))
            {
                throw new Exception("YAMLヘッダーが見つかりません");
            }

            int yamlEnd = Array.FindIndex(lines, 1, l => l.Trim() == "---" || l.Trim() == string.Empty);
            if (yamlEnd == -1)
            {
                throw new Exception("YAMLヘッダーの終了が見つかりません");
            }

            var yamlText = string.Join("\n", lines[1..yamlEnd]);
            var sqlText = string.Join("\n", lines[(yamlEnd + 1)..]);

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(UnderscoredNamingConvention.Instance)
                .Build();

            var meta = deserializer.Deserialize<SqlMigration>(yamlText);
            meta.Sql = sqlText.Trim();

            return meta;
        }
    }
}
