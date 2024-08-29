namespace Fx.Games.Game.Chess
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;


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
            return (StringSegment input, out T result, out StringSegment remainder) =>
            {
                foreach (var p in parsers.Prepend(parser))
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