namespace Db.System.Collections.Generic
{
    /// <summary>
    /// Extension methods for <see cref="IReadOnlyDictionary{TKey, TValue}"/>
    /// </summary>
    public static class ReadOnlyDictionaryExtensions
    {
        public static bool TryGetValue<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> self, TKey key, out TValue value)
        {
            value = self.GetValueTry(key, out var contained);
            return contained;
        }
    }
}
