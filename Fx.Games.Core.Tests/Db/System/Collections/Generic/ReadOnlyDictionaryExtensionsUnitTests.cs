namespace Db.System.Collections.Generic
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for <see cref="ReadOnlyDictionaryExtensions"/>
    /// </summary>
    [TestClass]
    public sealed class ReadOnlyDictionaryExtensionsUnitTests
    {
        /// <summary>
        /// Retrieves a value from a <see langword="null"/> dictionary
        /// </summary>
        [TestMethod]
        public void TryGetValueNullDictionary()
        {
            IReadOnlyDictionary<string, string> dictionary = null;
            Assert.ThrowsException<global::System.ArgumentNullException>(() => 
                dictionary
                    .TryGetValue("a key", out var value));
        }
    }
}
