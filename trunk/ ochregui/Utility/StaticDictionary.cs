using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace OchreGui.Utility
{
    /// <summary>
    /// Represents a type of Dictionary that, after construction, has a static number of
    /// items.  Items cannot be added or removed, but they can be modified as normal.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public interface IStaticDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        /// <summary>
        /// Get or set the value of the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        TValue this[TKey key] { get; }
        /// <summary>
        /// Gets the number of items in this collection.
        /// </summary>
        int Count { get; }
        /// <summary>
        /// Gets a list of keys contained in this collection.
        /// </summary>
        Dictionary<TKey, TValue>.KeyCollection Keys { get; }
        /// <summary>
        /// Returns true if the specified key is contained in this collection.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool ContainsKey(TKey key);
        /// <summary>
        /// Returns true if the specified value is contained in this collection.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool ContainsValue(TValue value);
    }


    /// <summary>
    /// Represents an IStaticMap object that is constructed using an array
    /// of key-value pairs.  Once constructed, items cannot be added or removed.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class StaticDictionary<TKey, TValue> : 
        IStaticDictionary<TKey,TValue>, IEnumerable<KeyValuePair<TKey, TValue>>
    {
        /// <summary>
        /// Construct a StaticDictionary instance given an array of key value pairs.
        /// </summary>
        /// <param name="items"></param>
        public StaticDictionary(KeyValuePair<TKey, TValue>[] items)
        {
            dictionary = new Dictionary<TKey, TValue>();

            foreach (var itm in items)
            {
                dictionary.Add(itm.Key, itm.Value);
            }

            
        }

        /// <summary>
        /// Gets or sets the value with the specified key.  The key must be non-null, and must
        /// exist in this StaticDictionary or an exception will be thrown.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="key"/>
        /// is null.</exception>
        /// <exception cref="System.ArgumentException">Thrown when <paramref name="key"/>
        /// does not exist in the default items.</exception>
        public TValue this[TKey key]
        {
            get
            {
                if (!dictionary.ContainsKey(key))
                {
                    throw new ArgumentException("Specified key does not exist");
                }
                if (key == null)
                {
                    throw new ArgumentNullException("key");
                }

                return dictionary[key];
            }
            set
            {
                if (!dictionary.ContainsKey(key))
                {
                    throw new ArgumentException("Specified key does not exist");
                }
                if (key == null)
                {
                    throw new ArgumentNullException("key");
                }

                dictionary[key] = value;
            }
        }

        /// <summary>
        /// Gets the number of items contained in this StaticDictionary
        /// </summary>
        public int Count
        {
            get
            {
                return dictionary.Count;
            }
        }

        /// <summary>
        /// Gets a collection containing the keys.
        /// </summary>
        public Dictionary<TKey, TValue>.KeyCollection Keys
        {
            get
            {
                return dictionary.Keys;
            }
        }

        /// <summary>
        /// Gets a collection containing the values.
        /// </summary>
        public Dictionary<TKey, TValue>.ValueCollection Values
        {
            get
            {
                return dictionary.Values;
            }
        }

        /// <summary>
        /// Returns true if this StaticDictionary contains the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">Thrown when <paramref name="key"/>
        /// is null.</exception>
        public bool ContainsKey(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            return dictionary.ContainsKey(key);
        }

        /// <summary>
        /// Returns true if this StaticDictionary contains the specified value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool ContainsValue(TValue value)
        {
            return dictionary.ContainsValue(value);
        }

        /// <summary>
        /// Returns a string representation of this object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return dictionary.ToString();
        }

        /// <summary>
        /// Returns an enumerator that iterates through this collection.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through this collection.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        private Dictionary<TKey, TValue> dictionary;
    }
}
