namespace Db.System.Collections.Generic
{
    public static class DbReadOnlyDictionary
    {
        public static DbReadOnlyDictionary<TKey, TValue> Create<TKey, TValue>(global::System.Collections.Generic.IReadOnlyDictionary<TKey, TValue> dictionary)
        {
            return new DbReadOnlyDictionary<TKey, TValue>(dictionary);
        }
    }
}
