using System;

namespace Fx.Games.Game.Chess
{
    public record Square
    {
        public Square(int File, int Rank)
        {
            // if (File < 0 || File > 7)
            // {
            //     throw new ArgumentOutOfRangeException(nameof(File));
            // }
            this.File = File;
            this.Rank = Rank;
        }

        /// <summary>
        /// The file of the square, 0-7, representing the columns a-h, the X-axis
        /// </summary>
        public int File { get; }

        /// <summary>
        /// The rank of the square, 0-7, representing the rows 1-8, the Y-axis
        /// </summary>
        public int Rank { get; }

        override public string ToString() => $"{(char)(File + 'a')}{(char)(Rank + '1')}";

        public static implicit operator Square((int File, int Rank) value) => new Square(value.File, value.Rank);
    }
}

