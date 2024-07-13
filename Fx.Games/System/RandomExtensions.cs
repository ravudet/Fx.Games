namespace System
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    public static class RandomExtensions
    {
        public static T Choose<T>(this Random rng, IReadOnlyList<T> items)
        {
            var ix = rng.Next(0, items.Count);
            return items[ix];
        }
    }
}
