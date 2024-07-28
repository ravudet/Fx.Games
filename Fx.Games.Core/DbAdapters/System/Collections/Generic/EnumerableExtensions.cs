namespace DbAdapters.System.Collections.Generic
{
    /// <summary>
    /// Extension methods for <see cref="global::System.Collections.Generic.IEnumerable{T}"/> adapters to Db
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Converts a C# <see cref="global::System.Collections.Generic.IEnumerable{T}"/> into a Db <see cref="global::Db.System.Collections.Generic.IEnumerable{T}"/>
        /// </summary>
        /// <typeparam name="T">The type of the elements of the <see cref="global::Db.System.Collections.Generic.IEnumerable{T}"/></typeparam>
        /// <param name="cEnumerable">The C# enumerable that should be adapted</param>
        /// <returns>A Db enumerable that is adapted from <paramref name="cEnumerable"/></returns>
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
