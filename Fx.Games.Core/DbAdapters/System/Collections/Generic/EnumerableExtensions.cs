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
    }
}
