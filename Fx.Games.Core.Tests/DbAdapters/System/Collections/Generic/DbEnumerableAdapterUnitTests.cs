namespace DbAdapters.System.Collections.Generic
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for <see cref="DbEnumerableAdapter{T}"/>
    /// </summary>
    [TestClass]
    public sealed class DbEnumerableAdapterUnitTests
    {
        /// <summary>
        /// Creates a Db enumerable adapter from a <see langword="null"/> C# enumerable
        /// </summary>
        [TestMethod]
        public void NullEnumerable()
        {
            Assert.ThrowsException<global::System.ArgumentNullException>(() => new DbEnumerableAdapter<string>(
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                // SUPPRESSION test case for the null validation
                null
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                ));
        }

        /// <summary>
        /// Creates a Db enumerable adapter when the C# enumerable returns <see langword="null"/> enumerators
        /// </summary>
        [TestMethod]
        public void NullEnumerator()
        {
            var enumerable = new NullEnumeratorEnumerable().ToDb();
            Assert.ThrowsException<global::System.NullReferenceException>(() =>
            {
                foreach (var element in enumerable)
                {
                }
            });
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

        /// <summary>
        /// A C# enumerable that returns a <see langword="null"/> enumerator
        /// </summary>
        private sealed class NullEnumeratorEnumerable : global::System.Collections.Generic.IEnumerable<string>
        {
            /// <inheritdoc/>
            public global::System.Collections.Generic.IEnumerator<string> GetEnumerator()
            {
                return
#pragma warning disable CS8603 // Possible null reference return.
                    // SUPPRESSION test case for the null validation
                    null;
#pragma warning restore CS8603 // Possible null reference return.
            }

            /// <inheritdoc/>
            global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }
    }
}
