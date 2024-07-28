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
                return null; //// TODO is this the right behavior? what does foreach expect?
            }

            return new DbEnumeratorAdapter<T>(cEnumerator);
        }
    }
}
