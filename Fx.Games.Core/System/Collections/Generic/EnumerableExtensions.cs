namespace System.Collections.Generic
{
    using System.Linq;

    /// <summary>
    /// Extension methods for <see cref="IEnumerable{T}"/>
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="keyValuePairs"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> keyValuePairs) where TKey : notnull
        {
            if (keyValuePairs == null)
            {
                throw new ArgumentNullException(nameof(keyValuePairs));
            }

            return keyValuePairs.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public static ICovariantReadOnlyDictionary<TKey, TValue> ToCovariantReadOnlyDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> keyValuePairs)
        {
            if (keyValuePairs == null)
            {
                throw new ArgumentNullException(nameof(keyValuePairs));
            }

            return new CovariantDictionary<TKey, TValue>(keyValuePairs.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
        }

        private sealed class CovariantDictionary<TKey, TValue> : ICovariantReadOnlyDictionary<TKey, TValue>
        {
            private readonly IReadOnlyDictionary<TKey, TValue> dictionary;

            public CovariantDictionary(IReadOnlyDictionary<TKey, TValue> dictionary)
            {
                this.dictionary = dictionary;
            }

            public TValue this[TKey key] => this.dictionary[key];

            public IEnumerable<TValue> Values => this.dictionary.Values;
        }
    }
}
