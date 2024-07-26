namespace Db.System.Collections.Generic
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for <see cref="EnumerableExtensions"/>
    /// </summary>
    [TestClass]
    public sealed class EnumerableExtensionsUnitTests
    {
        /// <summary>
        /// Converts a <see langword="null"/> sequence to a dictionary
        /// </summary>
        [TestMethod]
        public void ToDictionarySelectorsNullSequence()
        {
            IEnumerable<global::System.Collections.Generic.KeyValuePair<string, string>> enumerable = null;
            Assert.ThrowsException<global::System.ArgumentNullException>(() => enumerable.ToDictionary(_ => _, _ => _));
        }
    }
}
