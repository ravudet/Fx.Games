namespace Fx.Games.Game.Chess
{
    using System.Collections.Generic;

    public record Offset(int X, int Y)
    {
        public static Offset N => new Offset(0, -1);
        public static Offset S => new Offset(0, 1);
        public static Offset E => new Offset(1, 0);
        public static Offset W => new Offset(-1, 0);
        public static Offset NE => new Offset(1, -1);

        public static Offset SE => new Offset(1, 1);

        public static Offset SW => new Offset(-1, 1);

        public static Offset NW => new Offset(-1, -1);

        public readonly static Offset[] All = { Offset.N, Offset.E, Offset.S, Offset.W, Offset.NE, Offset.SE, Offset.SW, Offset.NW };

        public static Square operator +(Square sq, Offset dir) => new Square(sq.File + dir.X, sq.Rank + dir.Y);

        public static implicit operator Offset((int X, int Y) tuple) => new Offset(tuple.X, tuple.Y);
    }
}

