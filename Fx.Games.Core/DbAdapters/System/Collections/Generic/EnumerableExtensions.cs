namespace DbAdapters.System.Collections.Generic
{
    /// <summary>
    /// TODO
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// TODO
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cEnumerable"></param>
        /// <returns></returns>
        /// <exception cref="global::System.ArgumentNullException">Thrown if <paramref name="cEnumerable"/> is <see langword="null"/></exception>
        public static global::Db.System.Collections.Generic.IEnumerable<T> ToDb<T>(this global::System.Collections.Generic.IEnumerable<T> cEnumerable)
        {
            if (cEnumerable == null)
            {
                throw new global::System.ArgumentNullException(nameof(cEnumerable));
            }

            return new DbEnumerableAdapter<T>(cEnumerable);
        }

        /*/// <summary>
        /// TODO
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="self"/> is <see langword="null"/></exception>
        /// <exception cref="DuplicateKeyException">Thrown if any key/value pair in <paramref name="self"/> has the same key as another key/value pair in <paramref name="self"/></exception>
        public static global::Db.System.Collections.Generic.Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this global::System.Collections.Generic.IEnumerable<global::System.Collections.Generic.KeyValuePair<TKey, TValue>> self)
        {
            if (self == null)
            {
                throw new global::System.ArgumentNullException(nameof(self));
            }

            return self.ToDictionary(keyValuePair => keyValuePair.Key, keyValuePair => keyValuePair.Value);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="self"></param>
        /// <param name="keySelector"></param>
        /// <param name="valueSelector"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="self"/> or <paramref name="keySelector"/> or <paramref name="valueSelector"/> is <see langword="null"/></exception>
        /// <exception cref="DuplicateKeyException">Thrown if any elements in <paramref name="self"/> contain duplicate keys as converted by <paramref name="keySelector"/></exception>
        public static global::Db.System.Collections.Generic.Dictionary<TKey, TValue> ToDictionary<TElement, TKey, TValue>(this global::System.Collections.Generic.IEnumerable<TElement> self, global::System.Func<TElement, TKey> keySelector, global::System.Func<TElement, TValue> valueSelector)
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

            var dictionary = new global::Db.System.Collections.Generic.Dictionary<TKey, TValue>();
            foreach (var element in self)
            {
                var key = keySelector(element);
                var value = valueSelector(element);
                dictionary.Add(key, value);
            }

            return dictionary;
        }*/
    }
}
