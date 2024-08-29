namespace Fx.Games.Game.Chess
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;

    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public class Board
    {
        public Board() : this(new Piece[Size.Width, Size.Height])
        {
            InitializeBoard(grid);
        }

        public Board(Piece[,] grid)
        {
            this.grid = grid;
        }
        private readonly Piece[,] grid;

        static class Size
        {
            public const int Width = 8;
            public const int Height = 8;
        }

        /// <summary>
        /// place the pieces on the board for the initial setup
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="size"></param>
        /// <exception cref="ArgumentException"></exception>
        private static void InitializeBoard(Piece[,] grid)
        {
            const string init = "rnbqkbnr|pppppppp|........|........|........|........|PPPPPPPP|RNBQKBNR";
            // const string init = "rnbqkbnr|........|........|........|........|........|........|RNBQKBNR";
            var pieces = init.Split('|');

            for (var x = 0; x < 8; x++)
            {
                for (var y = 0; y < 8; y++)
                {
                    grid[x, y] = pieces[7 - y][x]; // implicit conversion from char to Tile
                }
            }
        }

        public override string ToString()
        {
            var (w, h) = (grid.GetLength(0), grid.GetLength(1));
            var len = ((w * 2) + 6) * (h + 2) - 4
            ;
            // one row on top and bottom each 
            // and two columns each on the left and right, plus the newlines
            return string.Create(len, grid, (span, grid) =>
            {
#if DEBUG
                span.Fill('?');
#endif
                Header(ref span, w);
                span.Append('\n');
                for (var y = h - 1; y >= 0; y--)
                {
                    var row = (char)(y + '1');
                    span.Append(row);
                    span.Append(' ');
                    for (var x = 0; x < w; x++)
                    {
                        span.Append(' ');
                        span.Append(grid[x, y].ToString());
                    }
                    span.Append("  ");
                    span.Append(row);
                    span.Append('\n');
                }
                span.Append('\n');
                Header(ref span, w);
            });
        }

        private static Span<char> Header(ref Span<char> span, int w)
        {
            span.Append("  ");
            for (var x = 0; x < w; x++)
            {
                span.Append(' ');
                span.Append((char)(x % 10 + 'a'));
            }
            span.Append('\n');
            return span;
        }

        public ref Piece this[Square sq] => ref grid[sq.File, sq.Rank];

        private string GetDebuggerDisplay() => ToString();

        public IEnumerable<Move> GetMoves(Color color)
        {
            foreach (var square in GetSquaresOfColor(color))
            {
                var piece = this[square];
                var moves = piece.Kind switch
                {
                    PieceKind.Pawn => GetPawnMoves(square),
                    PieceKind.Rook => GetRookMoves(square),
                    PieceKind.Knight => GetKnightMoves(square),
                    PieceKind.Bishop => GetBishopMoves(square),
                    PieceKind.Queen => GetQueenMoves(square),
                    PieceKind.King => GetKingMoves(square),
                    _ => Enumerable.Empty<Move>()
                };
                foreach (var move in moves)
                {
                    yield return move;
                }
            }
        }

        private static readonly Dir[] ROOK = new Dir[] { Dir.N, Dir.E, Dir.S, Dir.W };
        private static readonly Dir[] BISHOP = new Dir[] { Dir.NE, Dir.SE, Dir.SW, Dir.NW };
        private static readonly Dir[] QUEEN = new Dir[] { Dir.N, Dir.E, Dir.S, Dir.W, Dir.NE, Dir.SE, Dir.SW, Dir.NW };
        private static readonly Dir[] KING = new Dir[] { Dir.N, Dir.E, Dir.S, Dir.W, Dir.NE, Dir.SE, Dir.SW, Dir.NW };


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IEnumerable<Move> GetRookMoves(Square square)
        {
            return GetMovesInDirs(square, ROOK);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IEnumerable<Move> GetBishopMoves(Square square)
        {
            return GetMovesInDirs(square, BISHOP);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IEnumerable<Move> GetQueenMoves(Square square)
        {
            return GetMovesInDirs(square, QUEEN);
        }

        private IEnumerable<Move> GetKingMoves(Square square)
        {
            var piece = grid[square.File, square.Rank];
            var color = piece.Color;
            foreach (var dir in KING)
            {
                var dest = square + dir;
                if (!IsOnBoard(dest)) { continue; } // next dir if off the board

                var destination = grid[dest.File, dest.Rank];
                if (destination == Piece.Empty)
                {
                    yield return new Move(square, dest);
                }
                else if (destination.Color != color)
                {
                    // capture
                    yield return new Move(square, dest, true);
                    continue;
                }
            }
        }

        private static readonly Dir[] KNIGHT = new Dir[] { (1, 2), (-1, 2), (1, -2), (-1, -2), (2, 1), (2, -1), (-2, 1), (-2, -1) };

        private IEnumerable<Move> GetKnightMoves(Square square)
        {
            var piece = grid[square.File, square.Rank];
            var color = piece.Color;
            foreach (var dir in KNIGHT)
            {
                var dest = square + dir;
                if (!IsOnBoard(dest)) { continue; } // skip if off board
                var destination = grid[dest.File, dest.Rank];
                if (destination == Piece.Empty)
                {
                    yield return new Move(square, dest);
                }
                else if (destination.Color != color) // capture
                {
                    yield return new Move(square, dest, true);
                }
            }
        }

        private IEnumerable<Move> GetPawnMoves(Square square)
        {
            var color = grid[square.File, square.Rank].Color;
            var dir = color switch
            {
                Color.White => Dir.N,
                Color.Black => Dir.S,
                _ => throw new InvalidOperationException("Invalid color")
            };

            var dest = square + dir;
            if (IsOnBoard(dest) && grid[dest.File, dest.Rank] == Piece.Empty)
            {
                yield return new Move(square, dest);

                // check if pawn can move two squares
                var initialRow = color == Color.White ? 1 : 6;
                if (square.Rank == initialRow)
                {
                    var two = dest + dir;
                    if (IsOnBoard(two) && this[two] == Piece.Empty)
                    {
                        yield return new Move(square, two);
                    }
                }
            }

            // capture moves
            var oppositeColor = color == Color.White ? Color.Black : Color.White;
            var east = dest + Dir.E;
            if (IsOnBoard(east) && this[east].Color == oppositeColor)
            {
                yield return new Move(square, east, true);
            }
            var west = dest + Dir.W;
            if (IsOnBoard(west) && this[west].Color == oppositeColor)
            {
                yield return new Move(square, west, true);
            }
        }
        private IEnumerable<Move> GetMovesInDirs(Square square, Dir[] dirs)
        {
            var piece = grid[square.File, square.Rank];
            var color = piece.Color;
            foreach (var dir in dirs)
            {
                foreach (var dest in GetStraightPath(square, dir))
                {
                    var tile = grid[dest.File, dest.Rank];
                    if (tile == Piece.Empty)
                    {
                        yield return new Move(square, dest);
                    }
                    else if (tile.Color != color) // other player's piece: capture
                    {
                        yield return new Move(square, dest, true);
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private IEnumerable<Square> GetSquaresOfColor(Color color)
        {
            bool forward = color == Color.White;
            for (var y = forward ? 0 : Size.Height - 1; forward ? y < Size.Height : y > 0; y += forward ? 1 : -1)
            {
                for (var x = 0; x < Size.Width; x++)
                {
                    var tile = grid[x, y];
                    if (!tile.IsEmpty && tile.Color == color)
                    {
                        yield return new Square(x, y);
                    }
                }
            }
        }

        private static IEnumerable<Square> GetStraightPath(Square start, Dir dir)
        {
            for (var sq = start + dir; IsOnBoard(sq); sq += dir)
            {
                yield return sq;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsOnBoard(Square sq) =>
                    sq.File >= 0 && sq.File < Size.Width && sq.Rank >= 0 && sq.Rank < Size.Height;

        internal Board Apply(Move move)
        {
            var grid = (this.grid.Clone() as Piece[,])!;

            grid[move.Target.File, move.Target.Rank] = grid[move.From.File, move.From.Rank];
            grid[move.From.File, move.From.Rank] = Piece.Empty;

            return new Board(grid);
        }
    }
}
