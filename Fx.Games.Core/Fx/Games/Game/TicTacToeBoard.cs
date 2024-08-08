namespace Fx.Games.Game
{
    using System;

    /// <summary>
    /// A representation of the board state of a tic-tac-toe grid
    /// </summary>
    public sealed class TicTacToeBoard
    {
        /// <summary>
        /// The 3 by 3 grid of the current board state
        /// </summary>
        private readonly TicTacToePiece[,] grid;

        /// <summary>
        /// Initializes a new instance of the <see cref="TicTacToeBoard"/> class
        /// </summary>
        public TicTacToeBoard()
            : this(new TicTacToePiece[3, 3])
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TicTacToeBoard"/> class
        /// </summary>
        /// <param name="grid">The grid to set the board state to</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="grid"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="grid"/> is not a 3 by 3 grid</exception>
        public TicTacToeBoard(TicTacToePiece[,] grid)
        {
            if (grid == null)
            {
                throw new ArgumentNullException(nameof(grid));
            }

            if (grid.GetLength(0) != 3 || grid.GetLength(1) != 3)
            {
                throw new ArgumentOutOfRangeException(nameof(grid), "A 3x3 board is required");
            }

            this.grid = Clone(grid);
        }

        /// <summary>
        /// The 3 by 3 grid of the current board state
        /// </summary>
        public TicTacToePiece[,] Grid
        {
            get
            {
                return Clone(this.grid);
            }
        }

        private static TicTacToePiece[,] Clone(TicTacToePiece[,] grid)
        {
            var rows = grid.GetLength(0);
            var columns = grid.GetLength(1);
            var clone = new TicTacToePiece[rows, columns];
            for (int i = 0; i < rows; ++i)
            {
                for (int j = 0; j < columns; ++j)
                {
                    clone[i, j] = grid[i, j];
                }
            }

            return clone;
        }
    }
}
