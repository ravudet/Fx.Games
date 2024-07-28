namespace DbAdapters.System.Collections.Generic
{
    /// <summary>
    /// An adapter from a C# <see cref="global::System.Collections.Generic.IEnumerator{T}"/> to a Db <see cref="global::Db.System.Collections.Generic.IEnumerator{T}"/>
    /// </summary>
    /// <typeparam name="T">The type of the elements of the <see cref="global::Db.System.Collections.Generic.IEnumerator{T}"/></typeparam>
    public sealed class DbEnumeratorAdapter<T> : global::Db.System.Collections.Generic.IEnumerator<T>
    {
        /// <summary>
        /// The C# enumerator that should be adapter
        /// </summary>
        private readonly global::System.Collections.Generic.IEnumerator<T> cEnumerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbEnumeratorAdapter{T}"/> class
        /// </summary>
        /// <param name="cEnumerator">The C# enumerator that should be adapter</param>
        /// <exception cref="global::System.ArgumentNullException">Thrown if <paramref name="cEnumerator"/> is <see langword="null"/></exception>
        public DbEnumeratorAdapter(global::System.Collections.Generic.IEnumerator<T> cEnumerator)
        {
            if (cEnumerator == null)
            {
                throw new global::System.ArgumentNullException(nameof(cEnumerator));
            }

            this.cEnumerator = cEnumerator;
        }

        /// <inheritdoc/>
        public T Current
        {
            get
            {
                return this.cEnumerator.Current;
            }
        }

        /// <inheritdoc/>
        public bool MoveNext()
        {
            return this.cEnumerator.MoveNext();
        }
    }
}
