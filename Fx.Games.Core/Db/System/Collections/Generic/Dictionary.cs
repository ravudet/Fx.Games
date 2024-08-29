namespace Db.System.Collections.Generic
{
    /// <summary>
    /// A data structure that can look up values based on an associated key
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the collection</typeparam>
    /// <typeparam name="TValue">The type of the values associated with the keys</typeparam>
    public sealed class Dictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        /// <summary>
        /// The data store for all of the key/value pairs where the key is not <see langword="null"/>
        /// </summary>
#pragma warning disable CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
        // SUPPRESSION: we are explicitly handling null keys outselves to overcome the shortcoming of the .NET dictionary
        private readonly global::System.Collections.Generic.Dictionary<TKey, TValue> dictionary;
#pragma warning restore CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.

        /// <summary>
        /// The value (if any) for the <see langword="null"/> key
        /// </summary>
        private Nullable<TValue> nullValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="Dictionary{TKey, TValue}"/> class
        /// </summary>
        public Dictionary()
            : this(global::System.Collections.Generic.EqualityComparer<TKey>.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Dictionary{TKey, TValue}"/> class
        /// </summary>
        /// <param name="comparer">The <see cref="global::System.Collections.Generic.IEqualityComparer{T}"/> to use to compare the keys of the collection</param>
        /// <exception cref="global::System.ArgumentNullException">Thrown if <paramref name="comparer"/> is <see langword="null"/></exception>
        public Dictionary(global::System.Collections.Generic.IEqualityComparer<TKey> comparer)
        {
            if (comparer == null)
            {
                throw new global::System.ArgumentNullException(nameof(comparer));
            }

            this.dictionary = new global::System.Collections.Generic.Dictionary<
#pragma warning disable CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
                // SUPPRESSION: we are explicitly handling null keys outselves to overcome the shortcoming of the .NET dictionary
                TKey,
#pragma warning restore CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
                TValue>(comparer);
            this.nullValue = default;
        }

        /// <summary>
        /// Adds <paramref name="value"/> to the collection associated with <paramref name="key"/>
        /// </summary>
        /// <param name="key">The key that is being added to the collection</param>
        /// <param name="value">The value that is being added to the colleciton assocaited with <paramref name="key"/></param>
        /// <exception cref="DuplicateKeyException">Thrown if <paramref name="key"/> is already present in the collection</exception>
        public void Add(TKey key, TValue value)
        {
            if (key == null)
            {
                if (this.nullValue.HasValue)
                {
                    throw new DuplicateKeyException("TODO duplicate key");
                }
                else
                {
                    this.nullValue = new Nullable<TValue>(value);
                }
            }
            else
            {
                try
                {
                    this.dictionary.Add(key, value);
                }
                catch (global::System.ArgumentException e)
                {
                    throw new DuplicateKeyException("TODO duplicate key", e);
                }
            }
        }

        public global::System.Collections.Generic.IEnumerable<TKey> Keys
        {
            get
            {
                //// TODO this whole thing
                if (this.nullValue.HasValue)
                {
                    yield return default;
                }

                foreach (var key in this.dictionary.Keys)
                {
                    yield return key;
                }
            }
        }

        /// <inheritdoc/>
        public TValue GetValueTry(TKey key, out bool contained)
        {
            if (key == null)
            {
                if (this.nullValue.HasValue)
                {
                    contained = true;
                    return this.nullValue.Value;
                }
                else
                {
                    contained = false;
#pragma warning disable CS8603 // Possible null reference return.
                    // SUPPRESSION we intend to return null (if a reference type) in the case where the value is not contained in the dictionary
                    return default;
#pragma warning restore CS8603 // Possible null reference return.
                }
            }
            else
            {
                contained = this.dictionary.TryGetValue(key, out var value);
#pragma warning disable CS8603 // Possible null reference return.
                // SUPPRESSION if TValue is nullable, then this may be null, but in that case, the return type is nullable, so that's the correct behavior; if TValue is not nullable, then we couldn't add null to the dictionary in the first place
                return value;
#pragma warning restore CS8603 // Possible null reference return.
            }
        }
    }
}
