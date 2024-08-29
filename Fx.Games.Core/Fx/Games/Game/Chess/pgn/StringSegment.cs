using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Fx.Games.Game.Chess
{
    /// <summary>
    /// An optimized representation of a substring.
    /// </summary>
    [DebuggerDisplay("{Value}")]
    public readonly struct StringSegment : IEquatable<StringSegment>, IEquatable<string?>
    {
        /// <summary>
        /// Initializes an instance of the <see cref="StringSegment"/> struct.
        /// </summary>
        /// <param name="buffer">
        /// The original <see cref="string"/>. The <see cref="StringSegment"/> includes the whole <see cref="string"/>.
        /// </param>
        public StringSegment(string? buffer)
        {
            Buffer = buffer;
            Offset = 0;
            Length = buffer?.Length ?? 0;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="StringSegment"/> struct.
        /// </summary>
        /// <param name="buffer">The original <see cref="string"/> used as buffer.</param>
        /// <param name="offset">The offset of the segment within the <paramref name="buffer"/>.</param>
        /// <param name="length">The length of the segment.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="buffer"/> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="offset"/> or <paramref name="length"/> is less than zero, or <paramref name="offset"/> +
        /// <paramref name="length"/> is greater than the number of characters in <paramref name="buffer"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private StringSegment(string buffer, int offset, int length)
        {
            // Validate arguments, check is minimal instructions with reduced branching for inlinable fast-path
            // Negative values discovered though conversion to high values when converted to unsigned
            // Failure should be rare and location determination and message is delegated to failure functions
            if (buffer == null || (uint)offset > (uint)buffer.Length || (uint)length > (uint)(buffer.Length - offset))
            {
                ThrowInvalidArguments(buffer, offset, length);
            }

            Buffer = buffer;
            Offset = offset;
            Length = length;
        }

        // Methods that do no return (i.e. throw) are not inlined
        // https://github.com/dotnet/coreclr/pull/6103
        [DoesNotReturn]
        private static void ThrowInvalidArguments(string? buffer, int offset, int length)
        {
            // Only have single throw in method so is marked as "does not return" and isn't inlined to caller
            throw GetInvalidArgumentsException();

            Exception GetInvalidArgumentsException()
            {
                if (buffer == null)
                {
                    return new ArgumentNullException(nameof(buffer));
                }

                if (offset < 0)
                {
                    return new ArgumentOutOfRangeException(nameof(offset));
                }

                if (length < 0)
                {
                    return new ArgumentOutOfRangeException(nameof(length));
                }

                return new ArgumentException($"offset ({offset}) must not be larger than lenght ({length})");
            }
        }

        /// <summary>
        /// Gets the <see cref="string"/> buffer for this <see cref="StringSegment"/>.
        /// </summary>
        public string? Buffer { get; }

        /// <summary>
        /// Gets the offset within the buffer for this <see cref="StringSegment"/>.
        /// </summary>
        public int Offset { get; }

        /// <summary>
        /// Gets the length of this <see cref="StringSegment"/>.
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// Gets whether this <see cref="StringSegment"/> contains a valid value.
        /// </summary>
        [MemberNotNullWhen(true, nameof(Buffer))]
        [MemberNotNullWhen(true, nameof(Value))]
        public bool HasValue => Buffer != null;

        /// <summary>
        /// Gets the value of this segment as a <see cref="string"/>.
        /// </summary>
        public string? Value => HasValue ? Buffer.Substring(Offset, Length) : null;

        /// <summary>
        /// A <see cref="StringSegment"/> for <see cref="string.Empty"/>.
        /// </summary>
        public static readonly StringSegment Empty = string.Empty;


        /// <summary>
        /// Gets the <see cref="char"/> at a specified position in the current <see cref="StringSegment"/>.
        /// </summary>
        /// <param name="index">The offset into the <see cref="StringSegment"/></param>
        /// <returns>The <see cref="char"/> at a specified position.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is greater than or equal to <see cref="Length"/> or less than zero.
        /// </exception>
        public char this[int index]
        {
            get
            {
                if ((uint)index >= (uint)Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(index), $"index {index} must be less than {Length}");
                }

                Debug.Assert(Buffer is not null);
                return Buffer[Offset + index];
            }
        }

        /// <summary>
        /// Retrieves a <see cref="StringSegment"/> that represents a substring from this <see cref="StringSegment"/>.
        /// The <see cref="StringSegment"/> starts at the position specified by <paramref name="offset"/>.
        /// </summary>
        /// <param name="offset">The zero-based starting character position of a substring in this <see cref="StringSegment"/>.</param>
        /// <returns>A <see cref="StringSegment"/> that begins at <paramref name="offset"/> in this <see cref="StringSegment"/>
        /// whose length is the remainder.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="offset"/> is greater than or equal to <see cref="Length"/> or less than zero.
        /// </exception>
        public StringSegment Subsegment(int offset) => Subsegment(offset, Length - offset);

        /// <summary>
        /// Retrieves a <see cref="StringSegment"/> that represents a substring from this <see cref="StringSegment"/>.
        /// The <see cref="StringSegment"/> starts at the position specified by <paramref name="offset"/> and has the specified <paramref name="length"/>.
        /// </summary>
        /// <param name="offset">The zero-based starting character position of a substring in this <see cref="StringSegment"/>.</param>
        /// <param name="length">The number of characters in the substring.</param>
        /// <returns>A <see cref="StringSegment"/> that is equivalent to the substring of <paramref name="length"/> that begins at <paramref name="offset"/> in this <see cref="StringSegment"/></returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="offset"/> or <paramref name="length"/> is less than zero, or <paramref name="offset"/> + <paramref name="length"/> is
        /// greater than <see cref="Length"/>.
        /// </exception>
        public StringSegment Subsegment(int offset, int length)
        {
            if (!HasValue || offset < 0 || length < 0 || (uint)(offset + length) > (uint)Length)
            {
                throw new InvalidEnumArgumentException($"offset and length must be positive and offset + length > Length");
            }

            return new StringSegment(Buffer, Offset + offset, length);
        }

        /// <summary>
        /// Creates a new <see cref="StringSegment"/> from the given <see cref="string"/>.
        /// </summary>
        /// <param name="value">The <see cref="string"/> to convert to a <see cref="StringSegment"/></param>
        // Do NOT add a implicit converter from StringSegment to String. That would negate most of the perf safety.
        public static implicit operator StringSegment(string? value) => new StringSegment(value);

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><see langword="true" /> if the current object is equal to the other parameter; otherwise, <see langword="false" />.</returns>
        public bool Equals(StringSegment other) => Equals(other, StringComparison.Ordinal);


        /// <summary>
        /// Checks if the specified <see cref="string"/> is equal to the current <see cref="StringSegment"/>.
        /// </summary>
        /// <param name="text">The <see cref="string"/> to compare with the current <see cref="StringSegment"/>.</param>
        /// <returns><see langword="true" /> if the specified <see cref="string"/> is equal to the current <see cref="StringSegment"/>; otherwise, <see langword="false" />.</returns>
        public bool Equals(string? text) => Equals(text, StringComparison.Ordinal);



        /// <summary>
        /// Gets a <see cref="ReadOnlySpan{T}"/> from the current <see cref="StringSegment"/>.
        /// </summary>
        /// <returns>The <see cref="ReadOnlySpan{T}"/> from this <see cref="StringSegment"/>.</returns>
        public ReadOnlySpan<char> AsSpan() => Buffer.AsSpan(Offset, Length);


        /// <summary>
        /// Checks if the beginning of this <see cref="StringSegment"/> matches the specified <see cref="string"/> when compared using the specified <paramref name="comparisonType"/>.
        /// </summary>
        /// <param name="text">The <see cref="string"/>to compare.</param>
        /// <param name="comparisonType">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <returns><see langword="true" /> if <paramref name="text"/> matches the beginning of this <see cref="StringSegment"/>; otherwise, <see langword="false" />.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="text"/> is <see langword="null" />.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool StartsWith(string text, StringComparison comparisonType)
        {
            ArgumentNullException.ThrowIfNull(text, nameof(text));

            if (!HasValue)
            {
                if ((uint)comparisonType > (uint)StringComparison.OrdinalIgnoreCase)
                {
                    throw new ArgumentOutOfRangeException(nameof(comparisonType));
                }
                return false;
            }

            return AsSpan().StartsWith(text.AsSpan(), comparisonType);
        }
    }
}