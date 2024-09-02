namespace Fx.Games.Game.Chess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public readonly struct Piece : IEquatable<Piece>
    {
        private Piece(Color Color, Kind Kind, char Symbol)
        {
            this.Color = Color;
            this.Kind = Kind;
            this.Symbol = Symbol;
        }

        public static Piece WhiteKing { get; } = new Piece(Color.White, Kind.King, 'K');
        public static Piece WhiteQueen { get; } = new Piece(Color.White, Kind.Queen, 'Q');
        public static Piece WhiteRook { get; } = new Piece(Color.White, Kind.Rook, 'R');
        public static Piece WhiteBishop { get; } = new Piece(Color.White, Kind.Bishop, 'B');
        public static Piece WhiteKnight { get; } = new Piece(Color.White, Kind.Knight, 'N');
        public static Piece WhitePawn { get; } = new Piece(Color.White, Kind.Pawn, 'P');
        public static Piece BlackKing { get; } = new Piece(Color.Black, Kind.King, 'k');
        public static Piece BlackQueen { get; } = new Piece(Color.Black, Kind.Queen, 'q');
        public static Piece BlackRook { get; } = new Piece(Color.Black, Kind.Rook, 'r');
        public static Piece BlackBishop { get; } = new Piece(Color.Black, Kind.Bishop, 'b');
        public static Piece BlackKnight { get; } = new Piece(Color.Black, Kind.Knight, 'n');
        public static Piece BlackPawn { get; } = new Piece(Color.Black, Kind.Pawn, 'p');


        public Color Color { get; }
        public Kind Kind { get; }
        public char Symbol { get; }

        private readonly static IReadOnlyDictionary<char, Piece> SYMBOL_MAP = new[] {
            WhiteKing, WhiteQueen, WhiteRook, WhiteBishop, WhiteKnight, WhitePawn,
            BlackKing, BlackQueen, BlackRook, BlackBishop, BlackKnight, BlackPawn
        }.ToDictionary(piece => piece.Symbol);

        public static implicit operator char(Piece piece) => piece.Symbol;

        public static Piece FromChar(char ch) => SYMBOL_MAP[ch];

        public static implicit operator Piece(char ch) => SYMBOL_MAP[ch];

        public static bool TryParse(char ch, out Piece piece) => SYMBOL_MAP.TryGetValue(ch, out piece);

        public override string ToString() => $"{Color} {Kind}";

        #region Equality
        public bool Equals(Piece other)
        {
            return this.Color == other.Color && this.Kind == other.Kind;
        }

        public override bool Equals(object? obj)
        {
            return obj is Piece piece && Equals(piece);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Color, Kind);
        }
        public static bool operator ==(Piece left, Piece right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Piece left, Piece right)
        {
            return !(left == right);
        }
        #endregion
    }
}