
namespace Fx.Games.Game.Amazons
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Fx.Games.Game.Amazons.Extensions;

    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public class Board
    {
        public (int Width, int Height) Size { get; }

        private readonly Tile[,] grid;

        public Board((int Width, int Height) size)
        {
            Size = size;
            grid = new Tile[size.Width, size.Height];
            InitializeGrid(grid, size);
        }

        internal Board(Tile[,] grid)
        {
            Size = (grid.GetLength(0), grid.GetLength(1));
            this.grid = grid;
        }

        /// <summary>
        /// place the amazon on the board for the initial setup
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="size"></param>
        /// <exception cref="ArgumentException"></exception>
        private static void InitializeGrid(Tile[,] grid, (int Width, int Height) size)
        {
            if (!BoardSetup.TryGetValue(size, out var d))
            {
                throw new ArgumentException($"Invalid board size {size}. Supported sizes {string.Join(", ", BoardSetup.Keys)}");
            }

            grid.Fill(Tile.Empty);
            var (w, h) = size;
            grid[d, 0] = Tile.White;
            grid[w - d - 1, 0] = Tile.White;
            grid[0, d] = Tile.White;
            grid[w - 1, d] = Tile.White;

            grid[d, h - 1] = Tile.Black;
            grid[w - d - 1, h - 1] = Tile.Black;
            grid[0, h - 1 - d] = Tile.Black;
            grid[w - 1, h - 1 - d] = Tile.Black;
        }


        private static readonly Dictionary<(int, int), int> BoardSetup = new Dictionary<(int, int), int>
    {
        {(4, 5),    1},
        {(5, 5),    1},
        {(5, 6),    1},
        {(6, 6),    2},
        {(8, 8),    2},
        {(10, 10),  3},
        {(12, 12),  4},
    };

        public override string ToString()
        {
            var (w, h) = (grid.GetLength(0), grid.GetLength(1));
            var len = ((w * 2) + 4) * (h + 2) - 4;
            // one row on top and bottom each 
            // and two columns each on the left and right, plus the newlines
            return string.Create(len, grid, (span, grid) =>
            {
                span.Fill('?');
                Header(ref span, w);
                for (var y = h - 1; y >= 0; y--)
                {
                    // var row = (char)(y % 10 + '0');
                    var row = (y + 1).ToString()[0];
                    span.Append(row);
                    for (var x = 0; x < w; x++)
                    {
                        span.Append(' ');
                        span.Append(grid[x, y] switch
                        {
                            Tile.Black => 'B',
                            Tile.White => 'W',
                            Tile.Arrow => '*',
                            _ => '\u00b7'
                        });
                    }
                    span.Append(' ');
                    span.Append(row);
                    span.Append('\n');
                }
                Header(ref span, w);
            });
        }

        private static Span<char> Header(ref Span<char> span, int w)
        {
            span.Append(' ');
            for (var x = 0; x < w; x++)
            {
                span.Append(' ');
                span.Append((char)(x % 10 + 'a'));
            }
            span.Append('\n');
            return span;
        }

        public enum Tile
        {
            Empty,
            Black, White, Arrow
        }

        public ref Tile this[Square sq] => ref grid[sq.X, sq.Y];

        private string GetDebuggerDisplay() => ToString();

        public IEnumerable<Move> GetMoves(Tile tile)
        {
            foreach (var amzn in GetAmazons(tile))
            {
                foreach (var dest in GetStraightPath(amzn))
                {
                    foreach (var trgt in GetStraightPath(dest, ignore: amzn))
                    {
                        yield return new Move(amzn, dest, trgt);
                    }
                }
            }
        }

        private IEnumerable<Square> GetAmazons(Tile tile)
        {
            for (var x = 0; x < Size.Width; x++)
            {
                for (var y = 0; y < Size.Height; y++)
                {
                    if (grid[x, y] == tile)
                    {
                        yield return new Square(x, y);
                    }
                }
            }
        }

        private IEnumerable<Square> GetStraightPath(Square start, Square? ignore = null)
        {
            foreach (var dir in Dir.All)
            {
                foreach (var dest in GetStraightPath(start, dir))
                {
                    if (IsOnBoard(dest) && (grid[dest.X, dest.Y] == Tile.Empty || dest == ignore))
                    {
                        yield return dest;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private bool IsOnBoard(Square sq) =>
            sq.X >= 0 && sq.X < Size.Width && sq.Y >= 0 && sq.Y < Size.Height;


        private IEnumerable<Square> GetStraightPath(Square start, Dir dir)
        {
            for (var sq = start + dir; this.IsOnBoard(sq); sq += dir)
            {
                yield return sq;
            }
        }

        public Board Clone()
        {
            return new Board((Tile[,])grid.Clone());
        }
    }
}

