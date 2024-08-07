namespace Fx.Games.Game
{
    using System;

    /// <summary>
    /// A representation of the placing of an 'X' or an 'O' on a tic-tac-toe grid
    /// </summary>
    public sealed class TicTacToeMove
    {
        /// <summary>
        /// Initializes an instance of the <see cref="TicTacToeMove"/> class
        /// </summary>
        /// <param name="row">The row on the tic-tac-toe board to place the marker</param>
        /// <param name="column">The column on the tic-tac-toe board to place the marker</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="row"/> or <paramref name="column"/> goes out of the bounds of a tic-tac-toe grid</exception>
        public TicTacToeMove(uint row, uint column)
        {
            if (row > 2)
            {
                throw new ArgumentOutOfRangeException(nameof(row), "must be a value in [0-2]");
            }

            if (column > 2)
            {
                throw new ArgumentOutOfRangeException(nameof(column), "must be a value in [0-2]");
            }

            this.Row = row;
            this.Column = column;
        }

        /// <summary>
        /// The row on the tic-tac-toe board to place the marker
        /// </summary>
        public uint Row { get; }

        /// <summary>
        /// The column on the tic-tac-toe board to place the marker
        /// </summary>
        public uint Column { get; }
    }
}
