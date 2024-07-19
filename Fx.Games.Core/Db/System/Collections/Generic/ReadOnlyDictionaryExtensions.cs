namespace Db.System.Collections.Generic
{
    public static class ReadOnlyDictionaryExtensions
    {
        public static bool TryGetValue<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> self, TKey key, out TValue value)
        {
            value = self.GetValueTry(key, out var contained);
            return contained;
        }
    }
}
