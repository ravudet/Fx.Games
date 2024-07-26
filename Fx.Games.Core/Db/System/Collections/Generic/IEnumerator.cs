namespace Db.System.Collections.Generic
{
    public interface IEnumerator<out T>
    {
        T Current { get; }

        bool MoveNext();
    }
}
