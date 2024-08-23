namespace Fx.Games.Game.Amazons
{
    public readonly struct Direction
    {
        private readonly int x;
        private readonly int y;

        public Direction(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void Deconstruct(out int x, out int y)
        {
            x = this.x;
            y = this.y;
        }

        public static readonly Direction[] All = new[] {
            new Direction(1, 0),   // E
            new Direction(0, 1),   // N
            new Direction(-1, 0),  // W
            new Direction(0, -1),  // S  
            new Direction(1, 1),   // NE
            new Direction(-1, -1), // SW
            new Direction(1, -1),  // SE
            new Direction(-1, 1),  // NW
        };
    }
}

