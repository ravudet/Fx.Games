﻿namespace Fx.Games.Game
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Thrown when an illegal move is committed to a game
    /// </summary>
    /// <threadsafety instance="false"/>
    public sealed class IllegalMoveExeption : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IllegalMoveExeption"/> class.
        /// </summary>
        public IllegalMoveExeption()
            : base()
        {
        }

        /// <summary>
        /// Initializes an instance of the <see cref="IllegalMoveExeption"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for this exception.</param>
        public IllegalMoveExeption(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IllegalMoveExeption"/> class with a specified error message and the exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for this exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public IllegalMoveExeption(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IllegalMoveExeption"/> class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">An object that describes the source or destination of the serialized data.</param>
        public IllegalMoveExeption(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
