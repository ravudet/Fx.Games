namespace Fx.Games.Driver
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    /// <summary>
    /// Thrown when no strategy can be found for the current player of a game
    /// </summary>
    /// <threadsafety instance="false"/>
    public sealed class PlayerNotFoundExeption : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerNotFoundExeption"/> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public PlayerNotFoundExeption()
            : base()
        {
        }

        /// <summary>
        /// Initializes an instance of the <see cref="PlayerNotFoundExeption"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for this exception.</param>
        [ExcludeFromCodeCoverage]
        public PlayerNotFoundExeption(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerNotFoundExeption"/> class with a specified error message and the exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for this exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        [ExcludeFromCodeCoverage]
        public PlayerNotFoundExeption(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerNotFoundExeption"/> class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">An object that describes the source or destination of the serialized data.</param>
        [ExcludeFromCodeCoverage]
        public PlayerNotFoundExeption(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
