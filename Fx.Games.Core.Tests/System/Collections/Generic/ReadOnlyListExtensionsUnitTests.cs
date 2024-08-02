namespace System.Collections.Generic
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for <see cref="ReadOnlyListExtensions"/>
    /// </summary>
    [TestClass]
    public sealed class ReadOnlyListExtensionsUnitTests
    {
        /// <summary>
        /// Samples an element from <see langword="null"/> list
        /// </summary>
        [TestMethod]
        public void SampleNullList()
        {
            List<string> list = null;
            var random = new Random();

            Assert.ThrowsException<ArgumentNullException>(() => list.Sample(random));
        }

        /// <summary>
        /// Samples an element from a list using a <see langword="null"/> random
        /// </summary>
        [TestMethod]
        public void SampleNullRandom()
        {
            var list = new[] { "asdf" };
            Random random = null;

            Assert.ThrowsException<ArgumentNullException>(() => list.Sample(random));
        }

        /// <summary>
        /// Samples an element from an empty list
        /// </summary>
        [TestMethod]
        public void SampleEmptyList()
        {
            var list = new List<string>();
            var random = new Random();

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => list.Sample(random));
        }

        /// <summary>
        /// Samples an element from a list
        /// </summary>
        [TestMethod]
        public void Sample()
        {
            var list = new[] { "asdf" };
            var random = new Random();

            var sampled = list.Sample(random);

            Assert.AreEqual("asdf", sampled);
        }
    }
}
