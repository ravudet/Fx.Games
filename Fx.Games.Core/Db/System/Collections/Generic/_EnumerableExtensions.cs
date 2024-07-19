namespace System.Collections.Generic
{
    public static class _EnumerableExtensions
    {
        public static global::Db.System.Collections.Generic.Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> self)
        {
            return self.ToDictionary(keyValuePair => keyValuePair.Key, keyValuePair => keyValuePair.Value);
        }

        public static global::Db.System.Collections.Generic.Dictionary<TKey, TValue> ToDictionary<TElement, TKey, TValue>(this IEnumerable<TElement> self, Func<TElement, TKey> keySelector, Func<TElement, TValue> valueSelector)
        {
            var dictionary = new global::Db.System.Collections.Generic.Dictionary<TKey, TValue>();
            foreach (var element in self)
            {
                var key = keySelector(element);
                var value = valueSelector(element);
                dictionary.Add(key, value);
            }

            return dictionary;
        }
    }
}
