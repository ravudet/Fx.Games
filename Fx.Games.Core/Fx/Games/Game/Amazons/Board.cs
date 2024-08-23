
namespace Fx.Games.Game.Amazons
{
    using System;
    using System.Diagnostics;
    using System.Threading;

    /// <summary>
    /// Represents the possible states of a square on the Amazons board.
    /// </summary>
    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public sealed class Board
    {
        public static readonly (int Width, int Height) BOARD_SIZE = (10, 10);

        /// <summary>
        /// The 10 by 10 grid of the current board state
        /// 0,0 is bottom left
        /// </summary>
        private readonly SquareState[,] grid;

        public (int Width, int Height) Size => (grid.GetLength(0), grid.GetLength(1));

        /// <summary>
        /// Initializes a new instance of the <see cref="Board"/> class
        /// </summary>
        public Board()
            : this(new SquareState[BOARD_SIZE.Width, BOARD_SIZE.Height])
        {
            grid[0, 3] = SquareState.White; // a4
            grid[3, 0] = SquareState.White; // d1
            grid[6, 0] = SquareState.White; // g1
            grid[9, 3] = SquareState.White; // j4

            grid[0, 6] = SquareState.Black;
            grid[3, 9] = SquareState.Black;
            grid[6, 9] = SquareState.Black;
            grid[9, 6] = SquareState.Black;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Board"/> class
        /// </summary>
        /// <param name="grid">The grid to set the board state to</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="grid"/> is <see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="grid"/> is not a 3 by 3 grid</exception>
        public Board(SquareState[,] grid)
        {
            ArgumentNullException.ThrowIfNull(grid, nameof(grid));
            if (grid.GetLength(0) != BOARD_SIZE.Width || grid.GetLength(1) != BOARD_SIZE.Height) throw new ArgumentOutOfRangeException(nameof(grid), $"A {BOARD_SIZE.Width}x{BOARD_SIZE.Height} board is required");

            this.grid = grid;
        }

        // /// <summary>
        // /// The <see cref="BOARD_SIZE.Width"/> by <see cref="BOARD_SIZE.Height"/> grid of the current board state
        // /// </summary>
        // private SquareState[,] Grid => this.grid;

        public Board Clone()
        {

            return new Board((SquareState[,])this.grid.Clone());
        }

        public ref SquareState this[(int X, int Y) position]
        {
            get
            {
                var (x, y) = position;
                if (x < 0 || x >= BOARD_SIZE.Width) throw new ArgumentOutOfRangeException(nameof(x), $"must be between 0 and {BOARD_SIZE.Width}");
                if (y < 0 || y >= BOARD_SIZE.Height) throw new ArgumentOutOfRangeException(nameof(y), $"must be between 0 and {BOARD_SIZE.Height}");

                return ref this.grid[x, y];
            }
        }

        public SquareState this[int X, int Y]
        {
            get
            {
                if (X < 0 || X >= BOARD_SIZE.Width) throw new ArgumentOutOfRangeException(nameof(X), $"must be between 0 and {BOARD_SIZE.Width}");
                if (Y < 0 || Y >= BOARD_SIZE.Height) throw new ArgumentOutOfRangeException(nameof(Y), $"must be between 0 and {BOARD_SIZE.Height}");

                return this.grid[X, Y];
            }
        }

        private string GetDebuggerDisplay()
        {
            return string.Create((BOARD_SIZE.Width + 1) * BOARD_SIZE.Height, grid, (span, grid) =>
            {
                for (int y = 0; y < BOARD_SIZE.Height; y++)
                {
                    for (int x = 0; x < BOARD_SIZE.Width; x++)
                    {
                        span[y * (BOARD_SIZE.Width + 1) + x] = this.grid[x, y] switch
                        {
                            SquareState.Empty => '.',
                            SquareState.Black => 'B',
                            SquareState.White => 'W',
                            SquareState.Arrow => '*',
                            _ => throw new InvalidOperationException(),
                        };
                    }
                    span[y * (BOARD_SIZE.Width + 1) + BOARD_SIZE.Width] = '\n';
                }
            });
        }
    }
}

