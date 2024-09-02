
namespace Fx.Games.Game.Chess.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.Primitives;


    static class PgnMoveParser
    {
        static readonly TryParse<Kind> piece = Parser
            .OneOf("KQRBN")
            .Select(PieceFromChar)
            .Optional(() => Kind.Pawn);

        static Kind PieceFromChar(char ch)
        {
            return ch switch
            {
                'K' => Kind.King,
                'Q' => Kind.Queen,
                'R' => Kind.Rook,
                'B' => Kind.Bishop,
                'N' => Kind.Knight,
                _ => throw new ArgumentOutOfRangeException(nameof(ch), $"Invalid piece {ch}")
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
            select new Square((int)(file - 'a'), (int)('9' - rank));


        static readonly TryParse<PgnMove> kingSideCastle = Parser
            .String("O-O")
            .Select(_ => PgnMove.KingSideCastle);

        static readonly TryParse<PgnMove> queenSideCastle = Parser
            .String("O-O-O")
            .Select(_ => PgnMove.QueenSideCastle);

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

        static readonly TryParse<PgnMove> pgn = Parser.Or(
            queenSideCastle,
            kingSideCastle,
            withSource,
            withoutSource
        );

        public static bool Parse(string? input, out PgnMove result)
        {
            return pgn(input, out result, out var remainder);
        }

    }



    public delegate bool TryParse<T>(StringSegment input, out T result, out StringSegment remainder);


    static class Parser
    {
        public static TryParse<char> Char(char ch)
        {
            return (StringSegment input, out char result, out StringSegment remainder) =>
            {
                if (input.Length > 0 && ch == input[0])
                {
                    result = input[0];
                    remainder = input.Subsegment(1);
                    return true;
                }

                result = default;
                remainder = input;
                return false;
            };
        }

        public static TryParse<string> String(string str)
        {
            return (StringSegment input, out string result, out StringSegment remainder) =>
            {
                if (input.StartsWith(str, StringComparison.Ordinal))
                {
                    result = str;
                    remainder = input.Subsegment(str.Length);
                    return true;
                }

                result = default!;
                remainder = input;
                return false;
            };
        }

        public static TryParse<char> OneOf(HashSet<char> chars)
        {
            return (StringSegment input, out char result, out StringSegment remainder) =>
            {
                if (input.Length > 0 && chars.Contains(input[0]))
                {
                    result = input[0];
                    remainder = input.Subsegment(1);
                    return true;
                }

                result = default;
                remainder = input;
                return false;
            };
        }

        public static TryParse<char> OneOf(string chars) => OneOf(new HashSet<char>(chars));

        public static TryParse<T> Optional<T>(this TryParse<T> parser, Func<T> @default)
        {
            return (StringSegment input, out T result, out StringSegment remainder) =>
            {
                if (parser(input, out var value, out var rest))
                {
                    result = value;
                    remainder = rest;
                    return true;
                }

                result = @default();
                remainder = input;
                return true;
            };
        }

        public static TryParse<T?> Optional<T>(this TryParse<T> parser)
            where T : struct
        {
            return (StringSegment input, out T? result, out StringSegment remainder) =>
            {
                if (parser(input, out var value, out var rest))
                {
                    result = value;
                    remainder = rest;
                    return true;
                }

                result = default;
                remainder = input;
                return true;
            };
        }

        public static TryParse<R> Select<T, R>(this TryParse<T> parser, Func<T, R> selector)
        {
            return (StringSegment input, out R result, out StringSegment remainder) =>
            {
                if (parser(input, out var value, out var rest))
                {
                    result = selector(value);
                    remainder = rest;
                    return true;
                }

                result = default!;
                remainder = input;
                return false;
            };
        }

        public static TryParse<R> SelectMany<S, T, R>(this TryParse<S> parser, Func<S, TryParse<T>> parser2, Func<S, T, R> selector)
        {
            return (StringSegment input, out R result, out StringSegment remainder) =>
            {
                if (parser(input, out var value1, out var rest1))
                {
                    var p = parser2(value1);
                    if (p(rest1, out var value2, out var rest2))
                    {
                        result = selector(value1, value2);
                        remainder = rest2;
                        return true;
                    }
                }

                result = default!;
                remainder = input;
                return false;
            };
        }


        public static TryParse<T> Or<T>(this TryParse<T> parser, params TryParse<T>[] parsers)
        {
            parsers = parsers.Prepend(parser).ToArray();
            return (StringSegment input, out T result, out StringSegment remainder) =>
            {
                foreach (var p in parsers)
                {
                    if (p(input, out var value, out var rest))
                    {
                        result = value;
                        remainder = rest;
                        return true;
                    }
                }

                result = default!;
                remainder = input;
                return false;
            };
        }
    }
}
