namespace Db.System
{
    /// <summary>
    /// A structure that was either initialized with a value or initialized without a value
    /// </summary>
    /// <typeparam name="T">The type of the value that the instance was potentially initialized with</typeparam>
    internal struct Nullable<T>
    {
        /// <summary>
        /// The contained value
        /// </summary>
        private readonly T value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Nullable{T}"/> struct
        /// </summary>
        /// <param name="value">The value that is contained in the <see cref="Nullable{T}"/></param>
        public Nullable(T value)
        {
            this.value = value;

            this.HasValue = true;
        }

        /// <summary>
        /// The value that is contained in this <see cref="Nullable{T}"/>
        /// </summary>
        /// <exception cref="global::System.InvalidOperationException">Thrown if this <see cref="Nullable{T}"/> was not initialized with a value</exception>
        public T Value
        {
            get
            {
                if (this.HasValue)
                {
                    return this.value;
                }

                throw new global::System.InvalidOperationException("The 'Nullable<T>' was not initialized with a value");
            }
        }

        /// <summary>
        /// Whether or not this <see cref="Nullable{T}"/> was initialized with a value
        /// </summary>
        public bool HasValue { get; }
    }
}
