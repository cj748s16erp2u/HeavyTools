using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.ImportBase
{
    public class DictionaryItemProxy : IReadOnlyDictionary<string, object>
    {
        private Dictionary<string, object> values = new Dictionary<string, object>();

        public ImportDictionary Description { get; }

        public DictionaryItemProxy(ImportDictionary description)
        {
            this.Description = description ?? throw new ArgumentNullException(nameof(description));
        }

        public object KeyValue => this[this.Description.KeyField];

        public IEnumerable<string> Keys => ((IReadOnlyDictionary<string, object>)this.values).Keys;

        IEnumerable<object> IReadOnlyDictionary<string, object>.Values => ((IReadOnlyDictionary<string, object>)this.values).Values;

        public int Count => ((IReadOnlyCollection<KeyValuePair<string, object>>)this.values).Count;

        public object this[string name]
        {
            get
            {
                if (this.values.TryGetValue(name, out var value))
                {
                    return value;
                }

                return null;
            }
            set
            {
                this.values[name] = value;
            }
        }

        public bool ContainsKey(string key)
        {
            return ((IReadOnlyDictionary<string, object>)this.values).ContainsKey(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return ((IReadOnlyDictionary<string, object>)this.values).TryGetValue(key, out value);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, object>>)this.values).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.values).GetEnumerator();
        }
    }

    public class DictionaryProxy : IEnumerable<DictionaryItemProxy>
    {
        private List<DictionaryItemProxy> list = new List<DictionaryItemProxy>();

        public ImportDictionary Description { get; }

        private DictionaryProxy()
        {
        }

        public DictionaryProxy(ImportDictionary description)
        {
            this.Description = description ?? throw new ArgumentNullException(nameof(description));
        }

        public void Add(object[] values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var names = new List<string>
            {
                this.Description.KeyField
            };
            foreach (var f in this.Description.ValueFields)
            {
                names.Add(f.Field);
            }

            this.Add(names.ToArray(), values);
        }

        public void Add(string[] names, object[] values)
        {
            if (names is null)
            {
                throw new ArgumentNullException(nameof(names));
            }

            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (names.Length != values.Length)
            {
                throw new ArgumentException($"The length of '{nameof(names)}' and '{nameof(values)}' arguments doesn't match.");
            }

            var item = new DictionaryItemProxy(this.Description);

            for (var i = 0; i < names.Length; i++)
            {
                item[names[i]] = values[i];
            }

            this.list.Add(item);
        }

        public IEnumerator<DictionaryItemProxy> GetEnumerator()
        {
            return ((IEnumerable<DictionaryItemProxy>)this.list).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.list).GetEnumerator();
        }
    }
}
