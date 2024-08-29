using System;
using System.Diagnostics.CodeAnalysis;

namespace Fx.Games.Game.Amazons
{


    public record Move(Square Amazon, Square Destination, Square Target) : IParsable<Move>
    {
        /// </inhertitdoc>
        public static bool TryParse(string? text, IFormatProvider? _, [MaybeNullWhen(false)] out Move value)
        {
            if (text == null)
            {
                value = default;
                return false;
            }
            var parts = text.Split(' ');
            if (parts.Length == 3 && Square.TryParse(parts[0], _, out var src) && Square.TryParse(parts[1], _, out var dest) && Square.TryParse(parts[2], _, out var arrow))
            {
                value = new Move(src, dest, arrow);
                return true;
            }
            value = default;
            return false;
        }

        public static Move Parse(string s, IFormatProvider? provider) =>
            TryParse(s, provider, out var value) ? value : throw new FormatException("Invalid move format");

        public override string ToString() => $"{Amazon} {Destination} {Target}";
    }
}

