using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.UnityPlugins
{
    public class JsonObject : IDictionary<string, object>
    {
        public object this[string key]
        {
            get
            {
                return new object();
            }

            set
            {
            }
        }

        public int Count
        {
            get
            {
                return 0;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        public ICollection<string> Keys
        {
            get
            {
                return new List<String>();
            }
        }

        public ICollection<object> Values
        {
            get
            {
                return new List<object>();
            }
        }

        public void Add(KeyValuePair<string, object> item)
        {
        }

        public void Add(string key, object value)
        {
        }

        public void Clear()
        {
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return false;
        }

        public bool ContainsKey(string key)
        {
            return false;
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return new List<KeyValuePair<string, object>>() as IEnumerator<KeyValuePair<string, object>>;
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return true;
        }

        public bool Remove(string key)
        {
            return true;
        }

        public bool TryGetValue(string key, out object value)
        {
            value = new object();
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new List<int>() as IEnumerator;
        }
    }
}
