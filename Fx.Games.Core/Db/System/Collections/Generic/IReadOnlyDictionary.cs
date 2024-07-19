namespace Db.System.Collections.Generic
{
    public interface IReadOnlyDictionary<in TKey, out TValue>
    {
        TValue GetValueTry(TKey key, out bool contained);
    }
}
