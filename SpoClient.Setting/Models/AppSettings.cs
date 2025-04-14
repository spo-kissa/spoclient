using SpoClient.Setting.Repositories;
using System.Collections;
using System.Collections.Specialized;

namespace SpoClient.Setting.Models
{
    /// <summary>
    ///     Application settings class.
    /// </summary>
    public class AppSettings : IAppSettings, IEnumerable<string?>
    {
        /// <summary>
        ///     Singleton instance of the settings class.
        /// </summary>
        private Settings repository;


        /// <summary>
        ///     The settings collection.
        /// </summary>
        public StringDictionary settings;



        /// <summary>
        ///     The singleton instance of the settings class.
        /// </summary>
        /// <param name="repository"></param>
        public AppSettings(Settings repository)
        {
            this.repository = repository;

            this.settings = this.repository.GetAllDictionary();
        }


        public IEnumerator<string?> GetEnumerator()
        {
            foreach(var v in this.settings.Values)
            {
                yield return v?.ToString();
            }
        }


        public void Add(string key, string? value)
        {
            this.repository.Add(key, value);
            this.settings.Add(key, value);
        }


        public string? this[string key]
        {
            get
            {
                return this.settings[key];
            }
            set
            {
                if (this.settings.ContainsKey(key))
                {
                    this.repository.Update(key, value);
                    this.settings[key] = value;
                    return;
                }
                else
                {
                    this.repository.Add(key, value);
                    this.settings[key] = value;
                }
            }
        }


        public int Count
        {
            get
            {
                return this.settings.Count;
            }
        }


        public bool IsSynchronized
        {
            get
            {
                return this.settings.IsSynchronized;
            }
        }


        public object SyncRoot
        {
            get
            {
                return this.settings.SyncRoot;
            }
        }


        public ICollection<string> Keys
        {
            get
            {
                return [.. this.settings.Keys.Cast<string>()];
            }
        }


        public ICollection<string?> Values
        {
            get
            {
                return [.. this.settings.Values.Cast<string?>()];
            }
        }


        public bool ContainsKey(string key)
        {
            return this.settings.ContainsKey(key);
        }


        public bool ContainsValue(string? value)
        {
            return this.settings.ContainsValue(value);
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.settings.GetEnumerator();
        }


        public void Remove(string key)
        {
            this.repository.Delete(key);
            this.settings.Remove(key);
        }
    }
}
