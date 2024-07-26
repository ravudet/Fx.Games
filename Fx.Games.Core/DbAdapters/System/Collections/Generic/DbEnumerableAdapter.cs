﻿namespace DbAdapters.System.Collections.Generic
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class DbEnumerableAdapter<T> : global::Db.System.Collections.Generic.IEnumerable<T>
    {
        private readonly global::System.Collections.Generic.IEnumerable<T> cEnumerable;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cEnumerable"></param>
        /// <exception cref="global::System.ArgumentNullException">Thrown if <paramref name="cEnumerable"/> is <see langword="null"/></exception>
        public DbEnumerableAdapter(global::System.Collections.Generic.IEnumerable<T> cEnumerable)
        {
            if (cEnumerable == null)
            {
                throw new global::System.ArgumentNullException(nameof(cEnumerable));
            }

            this.cEnumerable = cEnumerable;
        }

        public DbEnumeratorAdapter<T> GetEnumerator()
        {
            var cEnumerator = this.cEnumerable.GetEnumerator();
            if (cEnumerator == null)
            {
                return null; //// TODO is this the right behavior? what does foreach expect?
            }

            return new DbEnumeratorAdapter<T>(cEnumerator);
        }

        global::Db.System.Collections.Generic.IEnumerator<T> global::Db.System.Collections.Generic.IEnumerable<T>.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
