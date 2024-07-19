namespace Db.System.Collections.Generic
{
    /// <summary>
    /// Thrown when a key/value pair is being added to a dictionary but the dictionary already contains an entry with that key
    /// </summary>
    /// <threadsafety instance="false"/>
    public sealed class DuplicateKeyException : global::System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IllegalMoveExeption"/> class.
        /// </summary>
        [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public DuplicateKeyException()
            : base()
        {
        }

        /// <summary>
        /// Initializes an instance of the <see cref="IllegalMoveExeption"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for this exception.</param>
        [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public DuplicateKeyException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IllegalMoveExeption"/> class with a specified error message and the exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for this exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public DuplicateKeyException(string message, global::System.Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IllegalMoveExeption"/> class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">An object that describes the source or destination of the serialized data.</param>
        [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public DuplicateKeyException(global::System.Runtime.Serialization.SerializationInfo info, global::System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {

        }
    }
}
