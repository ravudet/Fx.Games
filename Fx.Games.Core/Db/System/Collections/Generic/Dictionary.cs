using System.Collections;

namespace Db.System.Collections.Generic
{
    public sealed partial class Dictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>, IEnumerable
    {
        private readonly global::System.Collections.Generic.Dictionary<TKey, TValue> dictionary;

        private readonly DbNullable<TValue> nullValue;

        public Dictionary()
            : this(global::System.Collections.Generic.EqualityComparer<TKey>.Default)
        {
        }

        public Dictionary(global::System.Collections.Generic.IEqualityComparer<TKey> comparer)
        {
            this.dictionary = new global::System.Collections.Generic.Dictionary<TKey, TValue>();
            this.nullValue = default;
        }

        public void Add(TKey key, TValue value)
        {
            this.dictionary.Add(key, value);
        }

        public IEnumerator GetEnumerator()
        {
            throw new global::System.NotImplementedException();
        }

        public TValue GetValueTry(TKey key, out bool contained)
        {
            if (key == null)
            {
                if (this.nullValue.HasValue)
                {
                    contained = true;
                    return this.nullValue.Value;
                }
                else
                {
                    contained = false;
                    return default;
                }
            }
            else
            {
                contained = this.dictionary.TryGetValue(key, out var value);
                return value;
            }
        }
    }
}
