namespace DbAdapters.System.Collections.Generic
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Unit tests for <see cref="DbEnumerableAdapter{T}"/>
    /// </summary>
    [TestClass]
    public sealed class DbEnumerableAdapterUnitTests
    {
        [TestMethod]
        public void NullEnumerable()
        {
            Assert.ThrowsException<global::System.ArgumentNullException>(() => new DbEnumerableAdapter<string>(null));
        }

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

        private sealed class NullEnumeratorEnumerable : global::System.Collections.Generic.IEnumerable<string>
        {
            public global::System.Collections.Generic.IEnumerator<string> GetEnumerator()
            {
                return null;
            }

            global::System.Collections.IEnumerator global::System.Collections.IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }
    }
}
