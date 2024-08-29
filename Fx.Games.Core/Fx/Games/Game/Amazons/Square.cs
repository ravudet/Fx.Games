using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Fx.Games.Game.Amazons
{
    public partial record Square(int X, int Y) : IParsable<Square>
    {
        public static implicit operator (int X, int Y)(Square square) => (square.X, square.Y);
        public static implicit operator Square((int X, int Y) tuple) => new(tuple.X, tuple.Y);

        override public string ToString() => $"{(char)('a' + X)}{Y + 1}";

        public static bool TryParse(string? text, IFormatProvider? _, [MaybeNullWhen(false)] out Square value)
        {
            // TODO check that X and Y are in bounds
            if (text == null)
            {
                value = default;
                return false;
            }
            if (SquareRegex().Match(text).Success)
            {
                value = new Square(text[0] - 'a', int.Parse(text[1..]) - 1);
                return true;
            }
            else
            {
                var parts = text.Split(',');
                if (parts.Length != 2 && int.TryParse(parts[0], out var x) && int.TryParse(parts[1], out var y))
                {
                    value = new Square(x, y);
                    return true;
                }
                value = default;
                return false;
            }
        }

        public static Square Parse(string s, IFormatProvider? provider) =>
            TryParse(s, provider, out var value) ? value : throw new FormatException("Invalid square format");

        [GeneratedRegex(@"\w\d")]
        private static partial Regex SquareRegex();
    }
}

