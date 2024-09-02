namespace Fx.Games.Game.Chess
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

    [DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
    public readonly struct Occupancy : IEquatable<Occupancy>
    {
        private Occupancy(bool HasPiece, Piece piece)
        {
            this.HasPiece = HasPiece;
            this.Piece = piece;
        }

        /// <summary>
        /// constructor for occupied Squares
        /// </summary>
        /// <param name="piece"></param>
        public Occupancy(Piece piece) : this(true, piece) { }

        public static Occupancy Empty { get; } = new Occupancy(
            false, default
        );


        [MemberNotNullWhen(true, nameof(Piece))]
        public bool HasPiece { get; }

        public Piece Piece { get; }

        public static implicit operator Occupancy(Piece piece) =>
            new Occupancy(piece);

        public char Symbol => HasPiece ? Piece.Symbol : '.';


        #region Equality

        public static bool operator ==(Occupancy left, Occupancy right)
        {
            return left.HasPiece == right.HasPiece && (!right.HasPiece || left.Piece == right.Piece);
        }

        public static bool operator !=(Occupancy left, Occupancy right)
        {
            return !(left == right);
        }

        public bool Equals(Occupancy other)
        {
            return this == other;
        }

        public override bool Equals(object? obj)
        {
            return obj is Occupancy occupancy && Equals(occupancy);
        }

        public override int GetHashCode()
        {
            return HasPiece ? HashCode.Combine(Piece) : HashCode.Combine(0);
        }

        internal static Occupancy FromChar(char v)
        {
            if (v is '.' or ' ' or '\u00B7')
            {
                return Empty;
            }
            return Piece.FromChar(v);
        }

        private string GetDebuggerDisplay()
        {
            return HasPiece ? $"{Piece.Color} {Piece.Kind}" : "Empty";
        }

        #endregion
    }
}
