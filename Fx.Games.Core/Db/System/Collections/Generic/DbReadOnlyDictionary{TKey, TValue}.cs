namespace Db.System.Collections.Generic
{
    public sealed class DbReadOnlyDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        private readonly global::System.Collections.Generic.IReadOnlyDictionary<TKey, TValue> dictionary;

        public DbReadOnlyDictionary(global::System.Collections.Generic.IReadOnlyDictionary<TKey, TValue> dictionary)
        {
            this.dictionary = dictionary;
        }

        public TValue? GetValueTry(TKey key, out bool contained)
        {
            contained = this.dictionary.TryGetValue(key, out var value);
            return value;
        }
    }
}
