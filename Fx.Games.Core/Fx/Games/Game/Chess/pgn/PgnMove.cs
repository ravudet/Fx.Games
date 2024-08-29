namespace Fx.Games.Game.Chess
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;


    public record struct PgnMove(PieceKind Piece, Source From, bool Capture, Square Target) : IParsable<PgnMove>
    {
        public static PgnMove Parse(string s, IFormatProvider? provider)
        {
            return TryParse(s, provider, out var result) ? result : throw new FormatException();
        }

        public static bool TryParse([NotNullWhen(true)] string? input, IFormatProvider? provider, [MaybeNullWhen(false)] out PgnMove result)
        {
            return PgnMoveParser.Parse(input, out result);
        }

        internal bool Matches(Move move, Board board)
        {
            var tile = board[move.From];
            // System.Console.WriteLine($"Comparing {this} with {move} {tile}");
            return Target.Equals(move.Target) &&
                Piece == tile.Kind &&
                Capture == move.Capture &&
                (From.File == null || From.File.Value == move.From.File) &&
                (From.Rank == null || From.Rank.Value == move.From.Rank);
        }
    }

    static class PgnMoveParser
    {
        static readonly TryParse<PieceKind> piece = Parser
            .OneOf("KQRBN")
            .Select(PieceFromChar)
            .Optional(() => PieceKind.Pawn);

        static PieceKind PieceFromChar(char ch)
        {
            return ch switch
            {
                'K' => PieceKind.King,
                'Q' => PieceKind.Queen,
                'R' => PieceKind.Rook,
                'B' => PieceKind.Bishop,
                'N' => PieceKind.Knight,
                _ => throw new InvalidDataException($"Invalid piece {ch}")
            };
        }

        static readonly TryParse<Source> source =
            from file in Parser.OneOf("abcdefgh").Optional()
            from rank in Parser.OneOf("12345678").Optional()
            select new Source(
                file.HasValue ? file.Value - 'a' : null,
                rank.HasValue ? rank.Value - '1' : null);

        static readonly TryParse<Square> target =
            from file in Parser.OneOf("abcdefgh")
            from rank in Parser.OneOf("12345678")
            select new Square((int)(file - 'a'), (int)(rank - '1'));

        static readonly TryParse<PgnMove> kingSideCastle =
            Parser.String("O-O").Select(_ => new PgnMove(PieceKind.King, Source.None, false, new Square(6, 0)));

        static readonly TryParse<PgnMove> queenSideCastle =
                      Parser.String("O-O-O").Select(_ => new PgnMove(PieceKind.King, Source.None, false, new Square(6, 0)));


        static readonly TryParse<PgnMove> withSource =
            from p in piece
            from s in source
            from x in Parser.Char('x').Optional()
            from t in target
            select new PgnMove(p, s, x == 'x', t);

        static readonly TryParse<PgnMove> withoutSource =
            from p in piece
            from x in Parser.Char('x').Optional()
            from t in target
            select new PgnMove(p, Source.None, x == 'x', t);

        static readonly TryParse<PgnMove> pgn = Parser.Or(queenSideCastle, kingSideCastle, withSource, withoutSource);

        public static bool Parse(string? input, out PgnMove result)
        {
            return pgn(input, out result, out var remainder);
        }

    }


    public readonly record struct Source(int? File, int? Rank)
    {
        public static Source None { get; } = new Source(null, null);

        public override string ToString() => $"{(File != null ? (char)(File + 'a') : "")}{(Rank != null ? (char)(Rank + '1') : "")}";
    }

}
