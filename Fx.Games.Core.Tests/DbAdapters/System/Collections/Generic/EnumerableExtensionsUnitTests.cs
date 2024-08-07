namespace DbAdapters.System.Collections.Generic
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for <see cref="EnumerableExtensions"/>
    /// </summary>
    [TestClass]
    public sealed class EnumerableExtensionsUnitTests
    {
        /// <summary>
        /// Adapts a <see langword="null"/> C# enumerable into a Db enumerable
        /// </summary>
        [TestMethod]
        public void NullEnumerable()
        {
            global::System.Collections.Generic.IEnumerable<string> cEnumerable =
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                // SUPPRESSION test case for the null validation
                null;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            Assert.ThrowsException<global::System.ArgumentNullException>(() =>
#pragma warning disable CS8604 // Possible null reference argument.
                // SUPPRESSION test case for the null validation
                cEnumerable
#pragma warning restore CS8604 // Possible null reference argument.
                    .ToDb());
        }

        /// <summary>
        /// Adapts a C# enumerable into a Db enumerable and enumerates the elements
        /// </summary>
        [TestMethod]
        public void ForEach()
        {
            var list = new global::System.Collections.Generic.List<string>()
            {
                "asdf",
            }.ToDb();

            var count = 0;
            foreach (var element in list)
            {
                if (element == "asdf")
                {
                    ++count;
                }
            }

            Assert.AreEqual(1, count);
        }
    }
}
