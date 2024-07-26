namespace DbAdapters.System.Collections.Generic
{
    public sealed class DbEnumeratorAdapter<T> : global::Db.System.Collections.Generic.IEnumerator<T>
    {
        private readonly global::System.Collections.Generic.IEnumerator<T> cEnumerator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cEnumerator"></param>
        /// <exception cref="global::System.ArgumentNullException">Thrown if <paramref name="cEnumerator"/> is <see langword="null"/></exception>
        public DbEnumeratorAdapter(global::System.Collections.Generic.IEnumerator<T> cEnumerator)
        {
            if (cEnumerator == null)
            {
                throw new global::System.ArgumentNullException(nameof(cEnumerator));
            }

            this.cEnumerator = cEnumerator;
        }

        public T Current
        {
            get
            {
                return this.cEnumerator.Current;
            }
        }

        public bool MoveNext()
        {
            return this.cEnumerator.MoveNext();
        }
    }
}
