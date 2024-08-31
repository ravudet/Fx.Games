namespace Fx.Games.Game.Chess
{
    public record Move(Square Start, Square End, bool Capture = false)
    {
        override public string ToString()
        {
            if (IsCastle(out var side))
            {
                return $"{side}-side Castle";
            }
            return $"{Start} {End}{(Capture ? " x" : "")}";
        }

        public bool IsCastle(out Kind side)
        {
            // castle is represented as a kind moving 2 squares
            if (Start.File == 4 && // king is on e
                (Start.Rank == 0 || Start.Rank == 7) && // king is on 1 or 8 depending on color
                (End.File == 2 || End.File == 6) && // target is c or g
                End.Rank == Start.Rank)   // target is on the same rank
            {
                side = End.File == 2 ? Kind.Queen : Kind.King;
                return true;
            }
            side = default;
            return false;
        }
    }
}

