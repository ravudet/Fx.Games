namespace Db.System.Collections.Generic
{
    /// <summary>
    /// Iterates over a sequence of elemenmts
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequncce that is being iterated</typeparam>
    public interface IEnumerator<out T>
    {
        /// <summary>
        /// Gets the element in the sequence at the current position of the enumerator.
        /// </summary>
        T Current { get; }

        /// <summary>
        /// Advances the enumerator to the next element of the sequence.
        /// </summary>
        /// <returns><see langword="true"/> if the enumerator was successfully advanced to the next element; <see langword="null"/> if the enumerator has passed the end of the sequence.</returns>
        bool MoveNext();
    }
}
