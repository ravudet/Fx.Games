namespace DbAdapters.System.Collections.Generic
{
    /// <summary>
    /// An adapter from a C# <see cref="global::System.Collections.Generic.IEnumerable{T}"/> to a Db <see cref="Db.System.Collections.Generic.IEnumerable{T}"/>
    /// </summary>
    /// <typeparam name="T">The type of the elements of the <see cref="Db.System.Collections.Generic.IEnumerable{T}"/></typeparam>
    public sealed class DbEnumerableAdapter<T> : global::Db.System.Collections.Generic.IEnumerable<T>
    {
        /// <summary>
        /// The C# enumerable that should be adapted
        /// </summary>
        private readonly global::System.Collections.Generic.IEnumerable<T> cEnumerable;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbEnumerableAdapter{T}"/> class
        /// </summary>
        /// <param name="cEnumerable">The C# enumerable that should be adapted</param>
        /// <exception cref="global::System.ArgumentNullException">Thrown if <paramref name="cEnumerable"/> is <see langword="null"/></exception>
        public DbEnumerableAdapter(global::System.Collections.Generic.IEnumerable<T> cEnumerable)
        {
            if (cEnumerable == null)
            {
                throw new global::System.ArgumentNullException(nameof(cEnumerable));
            }

            this.cEnumerable = cEnumerable;
        }

        /// <inheritdoc/>
        public global::Db.System.Collections.Generic.IEnumerator<T> GetEnumerator()
        {
            var cEnumerator = this.cEnumerable.GetEnumerator();
            if (cEnumerator == null)
            {
#pragma warning disable CS8603 // Possible null reference return.
                // ienumerable.getenumerator isn't "supposed" to return null, but if it does, we should just pass that along and let the foreach (or whatever caller) treat it as it would normally treat a null (which is usually to make no such assertion, as is the case with foreach)
                return null;
#pragma warning restore CS8603 // Possible null reference return.
            }

            return new DbEnumeratorAdapter<T>(cEnumerator);
        }
    }
}
