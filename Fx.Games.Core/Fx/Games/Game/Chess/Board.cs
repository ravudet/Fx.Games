namespace Fx.Games.Game.Chess
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public class Board
    {
        private readonly Tile[,] grid;

        static class Size
        {
            public const int Width = 8;
            public const int Height = 8;
        }

        public Board()
        {
            grid = new Tile[Size.Width, Size.Height];
            InitializeBoard(grid);
        }

        /// <summary>
        /// place the pieces on the board for the initial setup
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="size"></param>
        /// <exception cref="ArgumentException"></exception>
        private static void InitializeBoard(Tile[,] grid)
        {
            const string pieces = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";

            var (x, y) = (0, 0);
            foreach (var ch in pieces)
            {
                switch (ch)
                {
                    case 'R': grid[x, y] = Tile.WhiteRook; x += 1; break;
                    case 'N': grid[x, y] = Tile.WhiteKnight; x += 1; break;
                    case 'B': grid[x, y] = Tile.WhiteBishop; x += 1; break;
                    case 'Q': grid[x, y] = Tile.WhiteQueen; x += 1; break;
                    case 'K': grid[x, y] = Tile.WhiteKing; x += 1; break;
                    case 'P': grid[x, y] = Tile.WhitePawn; x += 1; break;
                    case 'r': grid[x, y] = Tile.BlackRook; x += 1; break;
                    case 'n': grid[x, y] = Tile.BlackKnight; x += 1; break;
                    case 'b': grid[x, y] = Tile.BlackBishop; x += 1; break;
                    case 'q': grid[x, y] = Tile.BlackQueen; x += 1; break;
                    case 'k': grid[x, y] = Tile.BlackKing; x += 1; break;
                    case 'p': grid[x, y] = Tile.BlackPawn; x += 1; break;
                    case '.': grid[x, y] = Tile.Empty; break;
                    case '8': for (var i = 0; i < 8; i++) { grid[x, y] = Tile.Empty; x += 1; } break;
                    case '/': x = 0; y += 1; break;
                    default: throw new ArgumentException($"Invalid character '{ch}' in board setup string");
                }
            }
        }

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
                        span.Append(UnicodeSymbol(grid[x, y]));
                    }
                    span.Append(' ');
                    span.Append(row);
                    span.Append('\n');
                }
                Header(ref span, w);
            });
        }

        private static char Symbol(Tile tile) => tile switch
        {
            Tile.BlackKing => 'k',
            Tile.BlackQueen => 'q',
            Tile.BlackRook => 'r',
            Tile.BlackBishop => 'b',
            Tile.BlackKnight => 'n',
            Tile.BlackPawn => 'p',
            Tile.WhiteKing => 'K',
            Tile.WhiteQueen => 'Q',
            Tile.WhiteRook => 'R',
            Tile.WhiteBishop => 'B',
            Tile.WhiteKnight => 'N',
            Tile.WhitePawn => 'P',
            Tile.Empty => '\u00b7',
            _ => '?'
        };

        private static char UnicodeSymbol(Tile tile) => tile switch
        {
            Tile.BlackKing => '\u265A',
            Tile.BlackQueen => '\u265B',
            Tile.BlackRook => '\u265C',
            Tile.BlackBishop => '\u265D',
            Tile.BlackKnight => '\u265E',
            Tile.BlackPawn => '\u265F',

            Tile.WhiteKing => '\u2654',
            Tile.WhiteQueen => '\u2655',
            Tile.WhiteRook => '\u2656',
            Tile.WhiteBishop => '\u2657',
            Tile.WhiteKnight => '\u2658',
            Tile.WhitePawn => '\u2659',

            Tile.Empty => '\u00b7',
            _ => '?'
        };

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



        public ref Tile this[Square sq] => ref grid[sq.X, sq.Y];

        private string GetDebuggerDisplay() => ToString();

        public IEnumerable<Move> GetMoves(Color color)
        {
            foreach (var square in GetPieces(color))
            {
                foreach (var dest in GetStraightPath(square))
                {
                    yield return new Move(square, dest);
                }
            }
        }

        private IEnumerable<Square> GetPieces(Color color)
        {
            for (var x = 0; x < Size.Width; x++)
            {
                for (var y = 0; y < Size.Height; y++)
                {
                    var tile = grid[x, y];
                    if (!tile.IsEmpty() && tile.Color() == color)
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

        private IEnumerable<Square> GetStraightPath(Square start, Dir dir)
        {
            for (var sq = start + dir; this.IsOnBoard(sq); sq += dir)
            {
                yield return sq;
            }
        }

        private bool IsOnBoard(Square sq) =>
            sq.X >= 0 && sq.X < Size.Width && sq.Y >= 0 && sq.Y < Size.Height;
    }
}

