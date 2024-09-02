namespace Fx.Games.Game.Chess
{
    using System;

    /// <summary>
    /// Represents a square on the chess board
    /// top left index 0,0 is a8, bottom right index 7,7 is h1
    /// </summary>
    public readonly struct Square : IEquatable<Square>
    {
        /// <summary>
        /// File is the x-coordinate of the square, 0..9 = a..h starting from the left
        /// </summary>
        public int File { get; }

        /// <summary>
        /// Rank is the y-coordinate of the square, 0..7 -> 8..1 starting from the top
        /// </summary>
        public int Rank { get; }

        public Square(int file, int rank)
        {
            File = file;
            Rank = rank;
        }

        override public string ToString()
        {
            return $"{(char)('a' + File)}{(char)('8' - Rank)}";
        }

        public static Square operator +(Square square, (int x, int y) offset) =>
            new Square(square.File + offset.x, square.Rank + offset.y);

        public static implicit operator Square((int File, int Rank) tuple) => new Square(tuple.File, tuple.Rank);
        public static implicit operator (int File, int Rank)(Square square) => (square.File, square.Rank);

        #region Equality
        public bool Equals(Square other) => File == other.File && Rank == other.Rank;
        public override bool Equals(object? obj) => obj is Square square && Equals(square);
        public override int GetHashCode() => HashCode.Combine(File, Rank);
        public static bool operator ==(Square left, Square right) => left.Equals(right);
        public static bool operator !=(Square left, Square right) => !left.Equals(right);
        #endregion

        public static bool TryParse(string str, out Square square)
        {
            square = default;
            if (str.Length != 2)
            {
                return false;
            }

            int file = str[0] - 'a';
            int rank = '8' - str[1];

            if (file < 0 || file > 7 || rank < 0 || rank > 7)
            {
                return false;
            }

            square = new Square(file, rank);
            return true;
        }

        public static implicit operator Square(string str) => TryParse(str, out var square) ? square : throw new FormatException($"Invalud square notation {str}");
    }
}