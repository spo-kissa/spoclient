namespace SpoClient.Setting.Models
{
    public interface IAppSettings
    {
        string? this[string key] { get; set; }


        void Add(string key, string? value);


        int Count { get; }


        bool IsSynchronized { get; }


        object SyncRoot { get; }


        ICollection<string> Keys { get; }


        ICollection<string?> Values { get; }


        bool ContainsKey(string key);


        bool ContainsValue(string? value);


        void Remove(string key);
    }
}
