namespace Fx.Games.Game.Chess
{
    public record Dir(int X, int Y)
    {
        public static Dir N => new Dir(0, 1);
        public static Dir S => new Dir(0, -1);
        public static Dir E => new Dir(1, 0);
        public static Dir W => new Dir(-1, 0);
        public static Dir NE => new Dir(1, 1);

        public static Dir SE => new Dir(1, -1);

        public static Dir SW => new Dir(-1, -1);

        public static Dir NW => new Dir(-1, 1);

        public static IEnumerable<Dir> All => [Dir.N, Dir.E, Dir.S, Dir.W, Dir.NE, Dir.SE, Dir.SW, Dir.NW];

        public static Square operator +(Square sq, Dir dir) => new Square(sq.File + dir.X, sq.Rank + dir.Y);

        public static implicit operator Dir((int X, int Y) tuple) => new Dir(tuple.X, tuple.Y);
    }
}

