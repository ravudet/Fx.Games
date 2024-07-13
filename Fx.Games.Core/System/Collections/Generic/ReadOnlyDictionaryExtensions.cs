namespace System.Collections.Generic
{
    using System.Linq;

    /// <summary>
    /// Extension methods for <see cref="IReadOnlyDictionary{TKey, TValue}"/>
    /// </summary>
    public static class ReadOnlyDictionaryExtensions
    {
        /// <summary>
        /// Makes a copy of <paramref name="source"/> as a mutable object
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in <paramref name="source"/></typeparam>
        /// <typeparam name="TValue">The type of the values in <paramref name="source"/></typeparam>
        /// <param name="source">The <see cref="IReadOnlyDictionary{TKey, TValue}"/> to make a copy of</param>
        /// <returns>A copy of <paramref name="source"/></returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="source"/> is <see langword="null"/></exception>
        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> source) where TKey : notnull
        {
            //// TODO other overloads IEnumerable<KVP> for example
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public static IReadOnlyDictionary<TKey, TValue> AsReadOnlyDictionary<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> readOnlyDictionary)
        {
            return readOnlyDictionary;
        }
    }

    public interface ICovariantReadOnlyDictionary<in TKey, out TValue>
    {
        TValue this[TKey key] { get; }
    }
}
