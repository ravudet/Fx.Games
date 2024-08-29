namespace Fx.Games.Game.Chess
{
    using System;

    static class SpanExtensions
    {
        public static void Append(this ref Span<char> str, string value)
        {
            value.CopyTo(str[..value.Length]);
            str = str[value.Length..];
        }
        public static void Append(this ref Span<char> str, char value)
        {
            str[0] = value;
            str = str[1..];
        }

    }
}

