namespace Fx.Games.Game.Chess
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;


    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public class Board
    {
        public Board(Piece[,] grid)
        {
            this.grid = grid;
        }

        private readonly Piece[,] grid;

        public Board() : this(new Piece[Size.Width, Size.Height])
        {
            InitializeBoard(grid);
        }

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
            const string INIT = "rnbqkbnr|pppppppp|........|........|........|........|PPPPPPPP|RNBQKBNR";
            var pieces = INIT.Split('|');

            for (var x = 0; x < 8; x++)
            {
                for (var y = 0; y < 8; y++)
                {
                    grid[x, y] = (char)pieces[7 - y][x]; // conversion from char to Piece
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
                    Kind.Pawn => GetPawnMoves(square),
                    Kind.Rook => GetRookMoves(square),
                    Kind.Knight => GetKnightMoves(square),
                    Kind.Bishop => GetBishopMoves(square),
                    Kind.Queen => GetQueenMoves(square),
                    Kind.King => GetKingMoves(square),
                    _ => Enumerable.Empty<Move>()
                };
                foreach (var move in moves)
                {
                    yield return move;
                }
            }
        }

        private static readonly Dir[] ROOK = { Dir.N, Dir.E, Dir.S, Dir.W };
        private static readonly Dir[] BISHOP = { Dir.NE, Dir.SE, Dir.SW, Dir.NW
    };
        private static readonly Dir[] QUEEN = { Dir.N, Dir.E, Dir.S, Dir.W, Dir.NE, Dir.SE, Dir.SW, Dir.NW };
        private static readonly Dir[] KING = { Dir.N, Dir.E, Dir.S, Dir.W, Dir.NE, Dir.SE, Dir.SW, Dir.NW };

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
            if (square.File == 4 && square.Rank == 0)
            {
                // white king side castle
                if (grid[5, 0] == Piece.Empty && grid[6, 0] == Piece.Empty)
                {
                    yield return new Move(square, new Square(6, 0));
                }
                // white queen side castle
                if (grid[3, 0] == Piece.Empty && grid[2, 0] == Piece.Empty && grid[1, 0] == Piece.Empty)
                {
                    yield return new Move(square, new Square(2, 0));
                }
            }
            else if (square.File == 4 && square.Rank == 7)
            {
                // black king side castle
                if (grid[5, 7] == Piece.Empty && grid[6, 7] == Piece.Empty)
                {
                    yield return new Move(square, new Square(6, 7));
                }
                // black queen side castle
                if (grid[3, 7] == Piece.Empty && grid[2, 7] == Piece.Empty && grid[1, 7] == Piece.Empty)
                {
                    yield return new Move(square, new Square(2, 7));
                }
            }
        }

        private static readonly Dir[] KNIGHT = { (1, 2), (-1, 2), (1, -2), (-1, -2), (2, 1), (2, -1), (-2, 1), (-2, -1) };

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

        public Board Apply(Move move)
        {
            var grid = (this.grid.Clone() as Piece[,])!;

            grid[move.End.File, move.End.Rank] = grid[move.Start.File, move.Start.Rank];
            grid[move.Start.File, move.Start.Rank] = Piece.Empty;

            // also move the rook if it's a castle
            if (move.IsCastle(out var side))
            {
                var (src, tgt) = move.End.File switch
                {
                    2 => (new Square(0, move.End.Rank), new Square(3, move.End.Rank)),
                    6 => (new Square(7, move.End.Rank), new Square(5, move.End.Rank)),
                    _ => throw new InvalidOperationException("Invalid castle move")
                };
                grid[tgt.File, tgt.Rank] = grid[src.File, src.Rank];
                grid[src.File, src.Rank] = Piece.Empty;
            }
            // else if (move.IsCastle(Kind.Queen))
            // {
            //     var (src, tgt) = move.Target.File switch
            //     {
            //         2 => (new Square(0, move.Target.Rank), new Square(4, move.Target.Rank)),
            //         6 => (new Square(7, move.Target.Rank), new Square(5, move.Target.Rank)),
            //         _ => throw new InvalidOperationException("Invalid castle move")
            //     };
            //     grid[tgt.File, tgt.Rank] = grid[src.File, src.Rank];
            //     grid[src.File, src.Rank] = Piece.Empty;
            // }

            return new Board(grid);
        }

        public readonly struct Piece : IFormattable
        {
            public Piece(Color color, Kind kind)
            {
                this.Color = color;
                this.Kind = kind;
            }
            /// <summary>
            /// color of the piece on the tile, (Color)0 if it's empty
            /// </summary>
            public readonly Color Color { get; }

            /// <summary>
            /// Piece type of the piece on the tile, (Piece)0 if tile is empty
            /// </summary>
            public readonly Kind Kind { get; }

            public readonly bool IsEmpty => Color == 0 && Kind == 0;

            public static bool operator ==(Piece a, Piece b) => a.Kind == b.Kind && a.Color == b.Color;

            public static bool operator !=(Piece a, Piece b) => !(a == b);

            public override readonly bool Equals(object? obj) => obj is Piece piece && this == piece;

            public override readonly int GetHashCode() => HashCode.Combine(Color, Kind);

            public override readonly string ToString() => ToChar().ToString();

            public static Piece Empty => default;
            public static Piece BlackKing => new Piece(Color.Black, Kind.King);
            public static Piece BlackQueen => new Piece(Color.Black, Kind.Queen);
            public static Piece BlackRook => new Piece(Color.Black, Kind.Rook);
            public static Piece BlackBishop => new Piece(Color.Black, Kind.Bishop);
            public static Piece BlackKnight => new Piece(Color.Black, Kind.Knight);
            public static Piece BlackPawn => new Piece(Color.Black, Kind.Pawn);
            public static Piece WhiteKing => new Piece(Color.White, Kind.King);
            public static Piece WhiteQueen => new Piece(Color.White, Kind.Queen);
            public static Piece WhiteRook => new Piece(Color.White, Kind.Rook);
            public static Piece WhiteBishop => new Piece(Color.White, Kind.Bishop);
            public static Piece WhiteKnight => new Piece(Color.White, Kind.Knight);
            public static Piece WhitePawn => new Piece(Color.White, Kind.Pawn);

            public static implicit operator Piece(char ch) => ch switch
            {
                '.' => Empty,
                ' ' => Empty,
                '\u00B7' => Empty,
                'k' => BlackKing,
                'q' => BlackQueen,
                'r' => BlackRook,
                'b' => BlackBishop,
                'n' => BlackKnight,
                'p' => BlackPawn,
                'K' => WhiteKing,
                'Q' => WhiteQueen,
                'R' => WhiteRook,
                'B' => WhiteBishop,
                'N' => WhiteKnight,
                'P' => WhitePawn,
                _ => throw new ArgumentException($"Invalid character '{ch}'"),
            };

            public readonly char ToChar() => (this.Color, this.Kind) switch
            {
                ((Color)0, (Kind)0) => '\u00B7', // · middle dot
                (Color.Black, Kind.King) => 'k',  // ♚
                (Color.Black, Kind.Queen) => 'q',  // ♛
                (Color.Black, Kind.Rook) => 'r',  // ♜
                (Color.Black, Kind.Bishop) => 'b',  // ♝
                (Color.Black, Kind.Knight) => 'n',  // ♞
                (Color.Black, Kind.Pawn) => 'p',  // ♟
                (Color.White, Kind.King) => 'K',  // ♔
                (Color.White, Kind.Queen) => 'Q',  // ♕
                (Color.White, Kind.Rook) => 'R',  // ♖
                (Color.White, Kind.Bishop) => 'B',  // ♗
                (Color.White, Kind.Knight) => 'N',  // ♘
                (Color.White, Kind.Pawn) => 'P',  // ♙
                _ => throw new InvalidDataException($"Invalid piece {this.Color} {this.Kind}"),
            };

            public static implicit operator char(Piece tile) => tile.ToChar();

            public string ToString(string? format, IFormatProvider? formatProvider)
            {
                return format switch
                {
                    "L" => $"{Color} {Kind}",
                    _ => ToChar().ToString(),
                };
            }
        }
    }
}

