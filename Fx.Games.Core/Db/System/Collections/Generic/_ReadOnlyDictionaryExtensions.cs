namespace System.Collections.Generic
{
    using Db.System.Collections.Generic;

    public static class _ReadOnlyDictionaryExtensions
    {
        public static DbReadOnlyDictionary<TKey, TValue> ToDb<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> self)
        {
            return DbReadOnlyDictionary.Create(self);
        }
    }
}
