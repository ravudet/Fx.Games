namespace Db.System.Collections.Generic
{
    using DbAdapters.System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections.Generic;

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
            IEnumerable<string> enumerable = null;

            Assert.ThrowsException<global::System.ArgumentNullException>(() => enumerable.ToDictionary(_ => _, _ => _));
        }

        /// <summary>
        /// Converts a sequence to a dictionary using a <see langword="null"/> key selector
        /// </summary>
        [TestMethod]
        public void ToDictionarySelectorsNullKeySelector()
        {
            var enumerable = global::System.Linq.Enumerable.Empty<string>().ToDb();
            global::System.Func<string, string> keySelector = null;

            Assert.ThrowsException<global::System.ArgumentNullException>(() => enumerable.ToDictionary(keySelector, _ => _));
        }

        /// <summary>
        /// Converts a sequence to a dictionary using a <see langword="null"/> value selector
        /// </summary>
        [TestMethod]
        public void ToDictionarySelectorsNullValueSelector()
        {
            var enumerable = global::System.Linq.Enumerable.Empty<string>().ToDb();
            global::System.Func<string, string> valueSelector = null;

            Assert.ThrowsException<global::System.ArgumentNullException>(() => enumerable.ToDictionary(_ => _, valueSelector));
        }

        /// <summary>
        /// Converts a sequence to a dictionary
        /// </summary>
        [TestMethod]
        public void ToDictionarySelectors()
        {
            var enumerable = new[] { "asdf", "12345677" }.ToDb();
            var dictionary = enumerable.ToDictionary(element => element.Length, element => element);

            bool contained;
            string value;

            value = dictionary.GetValueTry(4, out contained);
            Assert.AreEqual(true, contained);
            Assert.AreEqual("asdf", value);

            value = dictionary.GetValueTry(8, out contained);
            Assert.AreEqual(true, contained);
            Assert.AreEqual("12345677", value);
        }

        /// <summary>
        /// Converts a sequence to a dictionary when duplicate keys are generated
        /// </summary>
        [TestMethod]
        public void ToDictionarySelectorsDuplicateKey()
        {
            var enumerable = new[] { "asdf", "qwer", "12345677" }.ToDb();

            Assert.ThrowsException<DuplicateKeyException>(() => enumerable.ToDictionary(element => element.Length, element => element));
        }

        /// <summary>
        /// Converts a <see langword="null"/> sequence to a dictionary
        /// </summary>
        [TestMethod]
        public void ToDictionaryNullSequence()
        {
            IEnumerable<global::System.Collections.Generic.KeyValuePair<string, string>> enumerable = null;

            Assert.ThrowsException<global::System.ArgumentNullException>(() => enumerable.ToDictionary());
        }

        [TestMethod]
        public void ToDictionary()
        {
            var enumerable = new[]
            {
                global::System.Collections.Generic.KeyValuePair.Create(4, "asdf"),
            }.ToDb();
            var dictionary = enumerable.ToDictionary();

            bool contained;
            string value;

            value = dictionary.GetValueTry(4, out contained);
            Assert.AreEqual(true, contained);
            Assert.AreEqual("asdf", value);
        }

        [TestMethod]
        public void ToDictionaryDuplicateKey()
        {
            var enumerable = new[]
            {
                global::System.Collections.Generic.KeyValuePair.Create(4, "asdf"),
                global::System.Collections.Generic.KeyValuePair.Create(4, "qwer"),
            }.ToDb();

            Assert.ThrowsException<DuplicateKeyException>(() => enumerable.ToDictionary());
        }
    }
}
