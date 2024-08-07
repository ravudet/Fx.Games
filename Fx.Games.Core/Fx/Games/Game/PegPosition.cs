namespace Fx.Games.Game
{
    using System;

    /// <summary>
    /// The position of a <see cref="Peg"/> in the triangle of a <see cref="PegGame{TPlayer}"/>
    /// </summary>
    public sealed class PegPosition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PegPosition"/> class
        /// </summary>
        /// <param name="row">The row (using 0-based indexing) in the triangle that the peg is at</param>
        /// <param name="column">The column (using 0-based indexing) within the <paramref name="row"/> that the peg is at</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="row"/> or <paramref name="column"/> is not within the 5-row triangle</exception>
        public PegPosition(int row, int column)
        {
            if (row < 0 || row > 4)
            {
                throw new ArgumentOutOfRangeException(nameof(row), $"'{nameof(row)}' must be a 0-based index within the 5-row triangle; the provided value was '{row}'.");
            }

            if (column < 0 || column > row)
            {
                throw new ArgumentOutOfRangeException(nameof(column), $"'{nameof(column)}' must be a 0-based index within the 5-row triangle; the provided row was '{row}' and the provided column was '{column}'");
            }

            this.Row = row;
            this.Column = column;
        }

        /// <summary>
        /// The row (using 0-based indexing) in the triangle that the peg is at
        /// </summary>
        public int Row { get; }

        /// <summary>
        /// The column (using 0-based indexing) within the <see cref="Row"/> that the peg is at
        /// </summary>
        public int Column { get; }
    }
}
