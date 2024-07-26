namespace Db.System.Collections.Generic
{
    public interface IEnumerable<out T>
    {
        IEnumerator<T> GetEnumerator();
    }
}
