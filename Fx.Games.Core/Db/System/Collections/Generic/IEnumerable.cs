namespace Db.System.Collections.Generic
{
    /// <summary>
    /// Elements that can be enumerated in sequence
    /// </summary>
    /// <typeparam name="T">The type of the elements that can be enumerated</typeparam>
    public interface IEnumerable<out T>
    {
        /// <summary>
        /// Returns an enumerator that iterates through the sequence of elements
        /// </summary>
        /// <returns>An <see cref="IEnumerator{T}"/> that can be used to iterate through the sequence of elements</returns>
        IEnumerator<T> GetEnumerator();
    }
}
