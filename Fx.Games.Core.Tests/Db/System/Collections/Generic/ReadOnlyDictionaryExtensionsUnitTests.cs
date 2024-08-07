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
            IReadOnlyDictionary<string, string> dictionary =
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                // SUPPRESSION test case for the null validation
                null;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            Assert.ThrowsException<global::System.ArgumentNullException>(() =>
#pragma warning disable CS8604 // Possible null reference argument.
                // SUPPRESSION test case for the null validation
                dictionary
#pragma warning restore CS8604 // Possible null reference argument.
                    .TryGetValue("a key", out var value));
        }
    }
}
