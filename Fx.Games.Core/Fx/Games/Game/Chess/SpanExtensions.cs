namespace Fx.Games.Game.Chess
{
    using System;

    static class SpanExtensions
    {
        public static void Append(this ref Span<char> span, string value)
        {
            value.AsSpan().CopyTo(span);
            span = span[value.Length..];
        }
        public static void Append(this ref Span<char> span, char c)
        {
            span[0] = c;
            span = span[1..];
        }
    }
}
