namespace Fx.Games.Game.Chess
{
    using System;
    using System.IO;


    public enum Color : byte { Black = 1, White }

    public enum PieceKind : byte { King = 1, Queen, Rook, Bishop, Knight, Pawn }
    public readonly struct Piece
    {
        public Piece(Color color, PieceKind piece) => (Color, Kind) = (color, piece);


        /// <summary>
        /// color of the piece on the tile, (Color)0 if it's empty
        /// </summary>
        public readonly Color Color { get; }

        /// <summary>
        /// Piece kind of the piece on the tile, (Piece)0 if tile is empty
        /// </summary>
        public readonly PieceKind Kind { get; }

        public readonly bool IsEmpty => Color == 0 && Kind == 0;

        public static bool operator ==(Piece a, Piece b) => a.Kind == b.Kind && a.Color == b.Color;

        public static bool operator !=(Piece a, Piece b) => !(a == b);

        public override readonly bool Equals(object? obj) => obj is Piece tile && this == tile;

        public override readonly int GetHashCode() => HashCode.Combine(Color, Kind);

        public override readonly string ToString() => ToChar().ToString();

        public static Piece Empty => default;
        public static Piece BlackKing => new Piece(Color.Black, PieceKind.King);
        public static Piece BlackQueen => new Piece(Color.Black, PieceKind.Queen);
        public static Piece BlackRook => new Piece(Color.Black, PieceKind.Rook);
        public static Piece BlackBishop => new Piece(Color.Black, PieceKind.Bishop);
        public static Piece BlackKnight => new Piece(Color.Black, PieceKind.Knight);
        public static Piece BlackPawn => new Piece(Color.Black, PieceKind.Pawn);
        public static Piece WhiteKing => new Piece(Color.White, PieceKind.King);
        public static Piece WhiteQueen => new Piece(Color.White, PieceKind.Queen);
        public static Piece WhiteRook => new Piece(Color.White, PieceKind.Rook);
        public static Piece WhiteBishop => new Piece(Color.White, PieceKind.Bishop);
        public static Piece WhiteKnight => new Piece(Color.White, PieceKind.Knight);
        public static Piece WhitePawn => new Piece(Color.White, PieceKind.Pawn);

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
            ((Color)0, (PieceKind)0) => '\u00B7', // · middle dot
            (Color.Black, PieceKind.King) => 'k',  // ♚
            (Color.Black, PieceKind.Queen) => 'q',  // ♛
            (Color.Black, PieceKind.Rook) => 'r',  // ♜
            (Color.Black, PieceKind.Bishop) => 'b',  // ♝
            (Color.Black, PieceKind.Knight) => 'n',  // ♞
            (Color.Black, PieceKind.Pawn) => 'p',  // ♟
            (Color.White, PieceKind.King) => 'K',  // ♔
            (Color.White, PieceKind.Queen) => 'Q',  // ♕
            (Color.White, PieceKind.Rook) => 'R',  // ♖
            (Color.White, PieceKind.Bishop) => 'B',  // ♗
            (Color.White, PieceKind.Knight) => 'N',  // ♘
            (Color.White, PieceKind.Pawn) => 'P',  // ♙
            _ => throw new InvalidDataException($"Invalid piece {this.Color} {this.Kind}"),
        };

        public static implicit operator char(Piece tile) => tile.ToChar();
    }

}