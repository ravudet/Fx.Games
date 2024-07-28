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
            global::System.Collections.Generic.IEnumerable<string> cEnumerable = null;

            Assert.ThrowsException<global::System.ArgumentNullException>(() => cEnumerable.ToDb());
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
