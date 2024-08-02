namespace System.Collections.Generic
{
    /// <summary>
    /// Extension methods for <see cref="IReadOnlyList{T}"/>
    /// </summary>
    public static class ReadOnlyListExtensions
    {
        /// <summary>
        /// Samples a value from <paramref name="self"/>
        /// </summary>
        /// <typeparam name="T">The type of the element to be sampled</typeparam>
        /// <param name="self">The list to sample an element of</param>
        /// <param name="random">The <see cref="Random"/> that represents the distribution that should be sampled</param>
        /// <returns>The value that was sampled</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="self"/> or <paramref name="random"/> is <see langword="null"/></exception>
        public static T Sample<T>(this IReadOnlyList<T> self, Random random)
        {
            if (self == null)
            {
                throw new ArgumentNullException(nameof(self));
            }

            if (random == null)
            {
                throw new ArgumentNullException(nameof(random));
            }

            var next = random.Next(0, self.Count);
            return self[next];
        }
    }
}
