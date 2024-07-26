namespace Db.System.Collections.Generic
{
    /// <summary>
    /// Extension methods for <see cref="IEnumerable{T}"/>
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Creates a <see cref="Dictionary{TKey, TValue}"/> from an <see cref="IEnumerable{T}"/> according to specified key selector and element selector functions.
        /// </summary>
        /// <typeparam name="TElement">The type of the elements of <paramref name="self"/></typeparam>
        /// <typeparam name="TKey">The type of the key returned by <paramref name="keySelector"/></typeparam>
        /// <typeparam name="TValue">The type of the value returned by <paramref name="valueSelector"/></typeparam>
        /// <param name="self">An <see cref="IEnumerable{T}"/> to create a <see cref="Dictionary{TKey, TValue}"/> from.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <param name="valueSelector">A transform function to produce a result element value from each element.</param>
        /// <returns>A <see cref="Dictionary{TKey, TValue}"/> that contains values of type <typeparamref name="TValue"/> selected from the input sequence.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="self"/> or <paramref name="keySelector"/> or <paramref name="valueSelector"/> is <see langword="null"/></exception>
        /// <exception cref="DuplicateKeyException">Thrown if any elements in <paramref name="self"/> contain duplicate keys as converted by <paramref name="keySelector"/></exception>
        public static Dictionary<TKey, TValue> ToDictionary<TElement, TKey, TValue>(this IEnumerable<TElement> self, global::System.Func<TElement, TKey> keySelector, global::System.Func<TElement, TValue> valueSelector)
        {
            if (self == null)
            {
                throw new global::System.ArgumentNullException(nameof(self));
            }

            if (keySelector == null)
            {
                throw new global::System.ArgumentNullException(nameof(keySelector));
            }

            if (valueSelector == null)
            {
                throw new global::System.ArgumentNullException(nameof(valueSelector));
            }

            var dictionary = new Dictionary<TKey, TValue>();
            foreach (var element in self)
            {
                var key = keySelector(element);
                var value = valueSelector(element);
                dictionary.Add(key, value);
            }

            return dictionary;
        }

        /// <summary>
        /// Creates a dictionary from an enumeration according to the default comparer for the key type.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys from elements of <paramref name="self"/>.</typeparam>
        /// <typeparam name="TValue">The type of the values from elements of <paramref name="self"/>.</typeparam>
        /// <param name="self">The enumeration to create a dictionary from.</param>
        /// <returns>A dictionary that contains keys and values from <paramref name="self"/> and uses the default comparer for the key type.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="self"/> is <see langword="null"/></exception>
        /// <exception cref="DuplicateKeyException">Thrown if any key/value pair in <paramref name="self"/> has the same key as another key/value pair in <paramref name="self"/></exception>
        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<global::System.Collections.Generic.KeyValuePair<TKey, TValue>> self)
        {
            if (self == null)
            {
                throw new global::System.ArgumentNullException(nameof(self));
            }

            return self.ToDictionary(keyValuePair => keyValuePair.Key, keyValuePair => keyValuePair.Value);
        }
    }
}
