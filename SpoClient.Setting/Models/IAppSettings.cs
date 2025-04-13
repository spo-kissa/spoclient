namespace SpoClient.Setting.Models
{
    /// <summary>
    ///     Application settings access object.
    /// </summary>
    public interface IAppSettings
    {
        /// <summary>
        ///     Get or set the value of the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string? this[string key] { get; set; }


        /// <summary>
        ///     Add a new key-value pair to the settings.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Add(string key, string? value);


        /// <summary>
        ///     Get the number of key-value pairs in the settings.
        /// </summary>
        int Count { get; }


        /// <summary>
        ///     Indicates whether access to the StringDictionary is synchronized (thread-safe).
        ///     This property is read only.
        /// </summary>
        bool IsSynchronized { get; }


        /// <summary>
        ///     Gets an object that can be used to synchronize access to the StringDictionary.
        /// </summary>
        object SyncRoot { get; }


        /// <summary>
        ///     Get all keys in the settings.
        /// </summary>
        ICollection<string> Keys { get; }


        /// <summary>
        ///     Get all values in the settings.
        /// </summary>
        ICollection<string?> Values { get; }


        /// <summary>
        ///     Determines if the string dictionary contains a specific key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool ContainsKey(string key);


        /// <summary>
        ///     Determines if the string dictionary contains a specific value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool ContainsValue(string? value);


        /// <summary>
        ///     Remove the specified key from the settings.
        /// </summary>
        void Remove(string key);
    }
}
