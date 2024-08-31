namespace Fx.Games.Game.Chess.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public record struct PgnMove(Kind Kind, Source Start, bool Capture, Square End) : IParsable<PgnMove>
    {

        public static PgnMove Parse(string s, IFormatProvider? provider)
        {
            return TryParse(s, provider, out var result) ? result : throw new FormatException();
        }

        public static bool TryParse([NotNullWhen(true)] string? input, IFormatProvider? provider, [MaybeNullWhen(false)] out PgnMove result)
        {
            return PgnMoveParser.Parse(input, out result);
        }

        public static PgnMove QueenSideCastle { get; } = new PgnMove(Kind.Queen, Source.None, false, new Square(2, -1));

        public static PgnMove KingSideCastle { get; } = new PgnMove(Kind.King, Source.None, false, new Square(6, -1));

        public readonly bool IsCastle(out Kind side)
        {
            if (this.End.Rank == -1)
            {
                side = this.Kind;
                return true;
            }
            side = default;
            return false;
        }


        // TODO 19. Bxb5 Rec8    Black Rec8    -> Black Rook O-O-O
        internal readonly bool Matches(Move move, Board board)
        {
            var tile = board[move.Start];
            if (this.IsCastle(out var side) && move.IsCastle(out var mside) && side == mside)
            {
                return true;
            }
            return End.Equals(move.End) &&
                Kind == tile.Kind &&
                Capture == move.Capture &&
                (Start.File == null || Start.File.Value == move.Start.File) &&
                (Start.Rank == null || Start.Rank.Value == move.Start.Rank);
        }

    }

    public readonly record struct Source(int? File, int? Rank)
    {
        public static Source None { get; } = new Source(null, null);

        public override string ToString() => $"{(File != null ? (char)(File + 'a') : "")}{(Rank != null ? (char)(Rank + '1') : "")}";
    }
}
