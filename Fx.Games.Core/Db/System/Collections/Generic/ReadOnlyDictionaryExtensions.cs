namespace Db.System.Collections.Generic
{
    /// <summary>
    /// Extension methods for <see cref="IReadOnlyDictionary{TKey, TValue}"/>
    /// </summary>
    public static class ReadOnlyDictionaryExtensions
    {
        /// <summary>
        /// Retrieves the value in <paramref name="self"/> that is associated with <paramref name="key"/>
        /// </summary>
        /// <param name="key">The <typeparamref name="TKey"/> to retrieve the associated value of</param>
        /// <param name="contained">The value that is associated with <paramref name="key"/> if <paramref name="key"/> is present in <paramref name="self"/>, otherwise the value is undefined</param>
        /// <returns>Whether or not <paramref name="self"/> contained <paramref name="key"/></returns>
        /// <exception cref="global::System.ArgumentNullException">Thrown if <paramref name="self"/> is <see langword="null"/></exception>
        public static bool TryGetValue<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> self, TKey key, out TValue value)
        {
            if (self == null)
            {
                throw new global::System.ArgumentNullException(nameof(self));
            }

            value = self.GetValueTry(key, out var contained);
            return contained;
        }
    }
}
