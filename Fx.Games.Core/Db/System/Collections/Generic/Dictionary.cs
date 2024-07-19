namespace Db.System.Collections.Generic
{
    public sealed partial class Dictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        private readonly global::System.Collections.Generic.Dictionary<TKey, TValue> dictionary;

        private readonly Nullable<TValue> nullValue;

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
