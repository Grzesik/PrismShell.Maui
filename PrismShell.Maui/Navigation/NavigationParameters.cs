using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prism.Navigation
{
    public class NavigationParameters : IEnumerable<KeyValuePair<string, object>>, INavigationParameters
    {
        private readonly List<KeyValuePair<string, object>> _entries = new List<KeyValuePair<string, object>>();

        public NavigationParameters()
        {
        }

        public object this[string key]
        {
            get
            {
                foreach (var entry in _entries)
                {
                    if (string.Compare(entry.Key, key, StringComparison.Ordinal) == 0)
                    {
                        return entry.Value;
                    }
                }

                return null;
            }
        }

        public int Count => _entries.Count;

        public IEnumerable<string> Keys =>
            _entries.Select(x => x.Key);

        public void Add(string key, object value) =>
           _entries.Add(new KeyValuePair<string, object>(key, value));

        public bool ContainsKey(string key) =>
            _entries.ContainsKey(key);

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() =>
            _entries.GetEnumerator();

        public T GetValue<T>(string key) =>
            _entries.GetValue<T>(key);

        /// <summary>
        /// Returns an IEnumerable of all parameters.
        /// </summary>
        /// <typeparam name="T">The type for the values to be returned.</typeparam>
        /// <param name="key">The key for the values to be returned.</param>
        ///<returns>Returns a IEnumerable of all the instances of type <typeparamref name="T"/>.</returns>
        public IEnumerable<T> GetValues<T>(string key) =>
            _entries.GetValues<T>(key);

        /// <summary>
        /// Checks to see if the parameter collection contains the value.
        /// </summary>
        /// <typeparam name="T">The type for the values to be returned.</typeparam>
        /// <param name="key">The key for the value to be returned.</param>
        /// <param name="value">Value of the returned parameter if it exists.</param>
        public bool TryGetValue<T>(string key, out T value) =>
            _entries.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        public void FromParameters(IEnumerable<KeyValuePair<string, object>> parameters) =>
            _entries.AddRange(parameters);

    }
}
