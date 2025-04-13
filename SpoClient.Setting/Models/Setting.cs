namespace SpoClient.Setting.Models
{
    public class Setting
    {
        public string Key { get; set; }


        public string? Value { get; set; } = null;



        public Setting(string key)
        {
            Key = key;
        }


        public Setting(string key, string? value)
        {
            Key = key;
            Value = value;
        }
    }
}
