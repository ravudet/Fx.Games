namespace System
{
    /// <summary>
    /// Utilites for the <see cref="Console"/> class
    /// </summary>
    public static class ConsoleUtilities
    {
        /// <summary>
        /// An object used to lock <see cref="Console"/> input, output, and redirect readers and writers for thread-safety purposes
        /// </summary>
        public static object ConsoleLock { get; } = new object();
    }
}
