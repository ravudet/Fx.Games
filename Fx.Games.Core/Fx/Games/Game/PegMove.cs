namespace Fx.Games.Game
{
    using System;

    /// <summary>
    /// The movement of a <see cref="Peg"/> from one <see cref="PegPosition"/> to another
    /// </summary>
    public sealed class PegMove
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PegMove"/> class
        /// </summary>
        /// <param name="start">The position that the peg starts at</param>
        /// <param name="end">The position that the peg should be moved to</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="start"/> or <paramref name="end"/> is <see langword="null"/></exception>
        public PegMove(PegPosition start, PegPosition end)
        {
            if (start == null)
            {
                throw new ArgumentNullException(nameof(start));
            }

            if (end == null)
            {
                throw new ArgumentNullException(nameof(end));
            }

            this.Start = start;
            this.End = end;
        }

        /// <summary>
        /// The position that the peg starts at
        /// </summary>
        public PegPosition Start { get; }

        /// <summary>
        /// The position that the peg should be moved to
        /// </summary>
        public PegPosition End { get; }
    }
}
