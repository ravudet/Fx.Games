namespace System.Collections.Generic
{
    public static class EnumerableExtensions
    {
        public static void ApplyToEmptyOrPopulated<T>(this IEnumerable<T> source, Action empty, Action<T> populated)
        {
            //// TODO this is a good example for either, maybe; how to go from ienumerable to either

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (empty == null)
            {
                throw new ArgumentNullException(nameof(empty));
            }

            if (populated == null)
            {
                throw new ArgumentNullException(nameof(populated));
            }

            using (var enumerator = source.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    empty();
                    return;
                }

                do
                {
                    populated(enumerator.Current);
                }
                while (enumerator.MoveNext());
            }
        }
    }
}
