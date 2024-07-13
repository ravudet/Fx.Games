namespace Db.System.Collections.Generic
{
    /// <summary>
    /// A data structure that can look up values based on an associated key
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the collection</typeparam>
    /// <typeparam name="TValue">The type of the values associated with the keys</typeparam>
    public interface IReadOnlyDictionary<in TKey, out TValue>
    {
        /// <summary>
        /// Retrieves the value in the collection that is associated with <paramref name="key"/>
        /// </summary>
        /// <param name="key">The <typeparamref name="TKey"/> to retrieve the associated value of</param>
        /// <param name="contained">Whether or not the collection contained <paramref name="key"/></param>
        /// <returns>The value that is associated with <paramref name="key"/> if <paramref name="key"/> is present in the collection, otherwise the value is undefined</returns>
        TValue GetValueTry(TKey key, out bool contained);
    }
}
