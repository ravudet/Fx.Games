namespace DbAdapters.System.Collections.Generic
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for <see cref="DbEnumeratorAdapter{T}"/>
    /// </summary>
    [TestClass]
    public sealed class DbEnumeratorAdapterUnitTests
    {
        /// <summary>
        /// Creates a Db enumerator adapter from a <see langword="null"/> C# enumerator
        /// </summary>
        [TestMethod]
        public void NullEnumerator()
        {
            Assert.ThrowsException<global::System.ArgumentNullException>(() => new DbEnumeratorAdapter<string>(null));
        }

        /// <summary>
        /// Adapts a C# enumerator into a Db enumerator and enumerates the elements
        /// </summary>
        [TestMethod]
        public void ForEach()
        {
            var list = new global::System.Collections.Generic.List<string>()
            {
                "asdf",
            };
            using (var cEnumerator = list.GetEnumerator())
            {
                var enumerator = new DbEnumeratorAdapter<string>(cEnumerator);

                var count = 0;
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current == "asdf")
                    {
                        ++count;
                    }
                }

                Assert.AreEqual(1, count);
            }
        }
    }
}
