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

        public int File { get; }
        public int Rank { get; }

        override public string ToString() => $"{(char)(File + 'a')}{(char)(Rank + '1')}";
    }
}

