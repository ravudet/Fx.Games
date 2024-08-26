namespace Fx.Games.Game.Chess
{

    public enum Tile
    {
        Empty = 0b0_0_0000,
        BlackKing = 0b1_0_0000, BlackQueen, BlackRook, BlackBishop, BlackKnight, BlackPawn,
        WhiteKing = 0b1_1_0000, WhiteQueen, WhiteRook, WhiteBishop, WhiteKnight, WhitePawn,
    }

    public static class TileExtensions
    {
        public static bool IsEmpty(this Tile tile) => tile == Tile.Empty;

        public static Color Color(this Tile tile) =>
            ((int)tile & 0b0_1_0000) == 0b0_1_0000 ? chess.Color.Black : chess.Color.White;

        public static Piece Piece(this Tile tile) =>
            (Piece)((int)tile & 0b1_0_0000);
    }
}

