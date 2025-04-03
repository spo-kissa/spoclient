namespace SpoClient.Setting
{
    public class SqlMigration
    {
        public string Name { get; set; } = string.Empty;


        public string Description { get; set; } = string.Empty;


        public string DependsOn {  get; set; } = string.Empty;


        public string Sql { get; set; } = string.Empty;
    }
}
