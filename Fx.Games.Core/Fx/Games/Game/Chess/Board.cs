namespace Fx.Games.Game.Chess
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class Board
    {
        readonly private Occupancy[,] grid = new Occupancy[8, 8];
        ulong whiteBitBoard = 0;
        ulong blackBitBoard = 0;

        public Board()
        {
            const string init = "rnbqkbnrpppppppp................................PPPPPPPPRNBQKBNR";
            var ix = 0;
            for (int rank = 0; rank < 8; rank++)
            {
                for (int file = 0; file < 8; file++)
                {
                    if (init[ix] != '.') { this[file, rank] = Occupancy.FromChar(init[ix]); }
                    ix += 1;
                }
            }
        }

        public Board(Occupancy[,] grid, ulong whiteBitBoard, ulong blackBitBoard)
        {
            this.grid = grid;
            this.whiteBitBoard = whiteBitBoard;
            this.blackBitBoard = blackBitBoard;
        }

        public Board Clone()
        {
            var grid = (Occupancy[,])this.grid.Clone();
            return new Board(grid, whiteBitBoard, blackBitBoard);
        }

        public Occupancy this[Square square]
        {
            // delegate to the indexer that takes two ints
            get => this[square.File, square.Rank];
            set => this[square.File, square.Rank] = value;
        }

        public Occupancy this[int file, int rank]
        {
            get => grid[file, rank];

            set
            {
                grid[file, rank] = value;
                Debug.Assert(grid[file, rank] == value);
                if (value.HasPiece)
                {
                    ref ulong bitBoard = ref (value.Piece.Color == Color.White ? ref whiteBitBoard : ref blackBitBoard);
                    bitBoard |= 1UL << (file * 8 + rank);
                    ref ulong other = ref (value.Piece.Color == Color.White ? ref blackBitBoard : ref whiteBitBoard);
                    other &= ~(1UL << (file * 8 + rank));
                }
                else
                {
                    // instead of figuring out which bitboard to clear, we clear both
                    whiteBitBoard &= ~(1UL << (file * 8 + rank));
                    blackBitBoard &= ~(1UL << (file * 8 + rank));
                }

                // ensure invariant: no overlapping bits are set   
                Debug.Assert((whiteBitBoard & blackBitBoard) == 0);
            }
        }

        override public string ToString()
        {
            int length = 200;
            return string.Create(length, grid, (span, grid) =>
            {
#if DEBUG 
                span.Fill('?'); // fill with ! for debugging in case one char is not set. otherwise they are invisible (\0)
#endif
                span.Append("  a b c d e f g h  \n");
                for (int rank = 0; rank < 8; rank++)
                {
                    span.Append((char)('0' + (8 - rank)));
                    span.Append(' ');
                    for (int file = 0; file < 8; file++)
                    {
                        var square = grid[file, rank];
                        span.Append(square.Symbol);
                        span.Append(' ');
                    }
                    span.Append((char)('0' + (8 - rank)));
                    span.Append('\n');
                }
                span.Append("  a b c d e f g h  \n");
            });
        }

        /// <summary>
        /// move a piece from start to end square in a straigt line
        /// either horizontally, vertically or diagonally
        /// no piece can be in the way
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <exception cref="InvalidOperationException"></exception>
        internal void MoveStraight(Square start, Square end)
        {
#if DEBUG
            if (this[start] == Occupancy.Empty)
            {
                throw new InvalidOperationException("No piece to move");
            }
            if (this[end] != Occupancy.Empty)
            {
                throw new InvalidOperationException("Cannot move to an occupied square");
            }
            var (dx, dy) = (Math.Abs(end.File - start.File), Math.Abs(end.Rank - start.Rank));
            if (!(dx == 0 || dy == 0 || dx == dy))
            {
                throw new InvalidOperationException("Invalid move, not horizontal, vertical or diagonal");
            }
            var Offset = (Math.Sign(end.File - start.File), Math.Sign(end.Rank - start.Rank));
            for (var sq = start + Offset; sq != end; sq += Offset)
            {
                if (this[sq] != Occupancy.Empty)
                {
                    throw new InvalidOperationException($"Invalid move, piece in the way: {this[sq].Piece} at {sq}");
                }
            }
#endif

            this[end] = this[start];
            this[start] = Occupancy.Empty;
        }

        // public override string ToString()
        // {
        //     var sb = new StringBuilder(20 * 10, 20 * 10);
        //     var w = new StringWriter();
        //     w.WriteLine("  a b c d e f g h");
        //     for (int i = 0; i < 8; i++)
        //     {
        //         w.Write("{0} ", 8 - i);
        //         for (int j = 0; j < 8; j++)
        //         {
        //             var square = grid[i, j];
        //             w.Write(square.Symbol);
        //             w.Write(" ");
        //         }
        //         w.WriteLine(" {0}", 8 - i);
        //     }
        //     w.WriteLine("  a b c d e f g h");
        //     System.Console.WriteLine(w.ToString().Length);
        //     return w.ToString();
        // }

        public Board CommitMove(Move move)
        {
            var board = this.Clone() as Board;
            board.MoveStraight(move.Start, move.End);
            return board;
        }

        #region Move generation

        public IEnumerable<Move> GetMoves(Color color)
        {
            foreach (var square in GetSquaresOfColor(color))
            {
                if (this[square].HasPiece)
                {
                    var piece = this[square].Piece;
                    var moves = piece.Kind switch
                    {
                        Kind.Pawn => GetPawnMoves(square),
                        Kind.Knight => GetKnightMoves(square),
                        Kind.Rook => GetMovesInStraightLines(square, ROOK),
                        Kind.Bishop => GetMovesInStraightLines(square, BISHOP),
                        Kind.Queen => GetMovesInStraightLines(square, QUEEN),
                        Kind.King => GetKingMoves(square),
                        _ => Enumerable.Empty<Move>()
                    };
                    foreach (var move in moves)
                    {
                        yield return move;
                    }
                }
            }
        }


        public IEnumerable<Square> GetSquaresOfColor(Color color)
        {
            // bool forward = color == Color.White;
            // for (var y = forward ? 0 : 8 - 1; forward ? y < 8 : y > 0; y += forward ? 1 : -1)
            for (var y = 0; y < 8; y++)
            {
                for (var x = 0; x < 8; x++)
                {
                    var o = grid[x, y];
                    if (o.HasPiece && o.Piece.Color == color)
                    {
                        yield return new Square(x, y);
                    }
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsOnBoard(Square sq) =>
                    sq.File >= 0 && sq.File < 8 && sq.Rank >= 0 && sq.Rank < 8;

        private IEnumerable<Move> GetPawnMoves(Square square)
        {
            Debug.Assert(this[square].HasPiece && this[square].Piece.Kind == Kind.Pawn);
            var piece = this[square].Piece;
            var color = piece.Color;
            var offset = piece.Color switch
            {
                Color.White => Offset.N,
                Color.Black => Offset.S,
                _ => throw new InvalidOperationException("Invalid color")
            };

            var dest = square + offset;
            if (IsOnBoard(dest) && this[dest] == Occupancy.Empty)
            {
                yield return new Move(square, dest);

                // check if pawn can move two squares
                var initialRow = color == Color.White ? 6 : 1;
                if (square.Rank == initialRow)
                {
                    var two = dest + offset;
                    if (IsOnBoard(two) && this[two] == Occupancy.Empty)
                    {
                        yield return new Move(square, two);
                    }
                }
            }
        }

        private static readonly Offset[] KNIGHT = new Offset[] { (1, 2), (-1, 2), (1, -2), (-1, -2), (2, 1), (2, -1), (-2, 1), (-2, -1) };

        private IEnumerable<Move> GetKnightMoves(Square square)
        {
            Debug.Assert(this[square].HasPiece && this[square].Piece.Kind == Kind.Knight, "Not a Knight at square");
            var piece = this[square].Piece;
            var color = piece.Color;
            foreach (var Offset in KNIGHT)
            {
                var dest = square + Offset;
                if (!IsOnBoard(dest)) { continue; } // skip if off board
                var destination = grid[dest.File, dest.Rank];
                if (destination == Occupancy.Empty)
                {
                    yield return new Move(square, dest);
                }
                else if (destination.Piece.Color != color) // capture
                {
                    yield return new Move(square, dest, true);
                }
            }
        }

        private IEnumerable<Move> GetKingMoves(Square square)
        {
            Debug.Assert(this[square].HasPiece); // && this[square].Piece.Kind == ??
            var piece = this[square].Piece;
            var color = piece.Color;

            foreach (var dir in KING)
            {
                var dest = square + dir;
                if (!IsOnBoard(dest)) { continue; } // next dir if off the board

                var destination = grid[dest.File, dest.Rank];
                if (destination == Occupancy.Empty)
                {
                    yield return new Move(square, dest);
                }
                else if (destination.Piece.Color != color)
                {
                    // capture
                    yield return new Move(square, dest, true);
                    continue;
                }
            }
        }

        private static readonly Offset[] ROOK = new Offset[] { Offset.N, Offset.E, Offset.S, Offset.W };
        private static readonly Offset[] BISHOP = new Offset[] { Offset.NE, Offset.SE, Offset.SW, Offset.NW };
        private static readonly Offset[] QUEEN = new Offset[] { Offset.N, Offset.E, Offset.S, Offset.W, Offset.NE, Offset.SE, Offset.SW, Offset.NW };
        private static readonly Offset[] KING = new Offset[] { Offset.N, Offset.E, Offset.S, Offset.W, Offset.NE, Offset.SE, Offset.SW, Offset.NW };


        private IEnumerable<Move> GetMovesInStraightLines(Square square, Offset[] Offsets)
        {
            Debug.Assert(this[square].HasPiece); // && this[square].Piece.Kind == ??
            var piece = this[square].Piece;
            var color = piece.Color;
            foreach (var Offset in Offsets)
            {
                foreach (var dest in GetSquaresOfStraightLines(square, Offset))
                {
                    var occupancy = grid[dest.File, dest.Rank];
                    // step to empty square
                    if (occupancy == Occupancy.Empty)
                    {
                        yield return new Move(square, dest);
                    }
                    // capture opposing color and stop
                    else if (occupancy.HasPiece && occupancy.Piece.Color != color)
                    {
                        yield return new Move(square, dest, true);
                        break;
                    }
                    // stop iterating
                    else
                    {
                        break;
                    }
                }
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IEnumerable<Square> GetSquaresOfStraightLines(Square start, Offset offset)
        {
            for (var sq = start + offset; IsOnBoard(sq); sq += offset)
            {
                yield return sq;
            }
        }
        #endregion
    }
}

