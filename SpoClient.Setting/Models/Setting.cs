namespace SpoClient.Setting.Models
{
    public class Setting
    {
        public string Key { get; set; } = string.Empty;


        public string Value { get; set; } = string.Empty;



        public Setting(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
