using System;

namespace Fx.Games.Game.Amazons
{
    /// <summary>
    /// A representation of the moving an amazon and placing an arrow on a board
    /// </summary>
    public sealed class Move
    {
        /// <summary>
        /// Initializes an instance of the <see cref="Move"/> class
        /// </summary>
        /// <param name="row">The row on the tic-tac-toe board to place the marker</param>
        /// <param name="column">The column on the tic-tac-toe board to place the marker</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="row"/> or <paramref name="column"/> goes out of the bounds of a tic-tac-toe grid</exception>
        public Move((int X, int Y) from, (int X, int Y) to, (int X, int Y) arrow)
        {
            if (!InBounds(from)) { throw new ArgumentOutOfRangeException(nameof(from), message); }
            if (!InBounds(to)) { throw new ArgumentOutOfRangeException(nameof(to), message); }
            if (!InBounds(arrow)) { throw new ArgumentOutOfRangeException(nameof(arrow), message); }

            this.From = from;
            this.To = to;
            this.Arrow = arrow;
        }

        private static readonly string message = $"must be a square on a {Board.BOARD_SIZE.Width}x{Board.BOARD_SIZE.Height} board";


        /// <summary>
        /// Checks if the given position is within the bounds of the Amazons board.
        /// </summary>
        /// <param name="position">The position to check.</param>
        /// <returns>True if the position is within the bounds, false otherwise.</returns>
        private static bool InBounds((int X, int Y) position)
        {
            return position.X >= 0 && position.X < Board.BOARD_SIZE.Width && position.Y >= 0 && position.Y < Board.BOARD_SIZE.Height;
        }

        public (int X, int Y) From { get; }
        public (int X, int Y) To { get; }
        public (int X, int Y) Arrow { get; }
    }
}

