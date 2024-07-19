namespace Db.System.Collections.Generic
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for <see cref="Dictionary{TKey, TValue}"/>
    /// </summary>
    [TestClass]
    public sealed class DictionaryUnitTests
    {
        /// <summary>
        /// Adds a key/value pair to a dictionary
        /// </summary>
        [TestMethod]
        public void AddAndRetrievePair()
        {
            var dictionary = new Dictionary<string, string>();
            dictionary.Add("a key", "some value");

            Assert.IsTrue(dictionary.TryGetValue("a key", out var value));
            Assert.AreEqual("some value", value);
        }

        /// <summary>
        /// Adds a duplicate key to a dictionary
        /// </summary>
        [TestMethod]
        public void AddDuplicateKey()
        {
            var dictionary = new Dictionary<string, string>();
            dictionary.Add("a key", "some value");
            Assert.ThrowsException<DuplicateKeyException>(() => dictionary.Add("a key", "some other value"));
        }

        /// <summary>
        /// Adds a key/value pair that has a <see langword="null"/> key
        /// </summary>
        [TestMethod]
        public void AddAndRetrieveNullKey()
        {
            var dictionary = new Dictionary<string?, string>();
            dictionary.Add(null, "some value");

            Assert.IsTrue(dictionary.TryGetValue(null, out var value));
            Assert.AreEqual("some value", value);
        }

        /// <summary>
        /// Adds a duplicate <see langword="null"/> key to a dictionary
        /// </summary>
        [TestMethod]
        public void AddDuplicateNullKey()
        {
            var dictionary = new Dictionary<string?, string>();
            dictionary.Add(null, "some value");
            Assert.ThrowsException<DuplicateKeyException>(() => dictionary.Add(null, "some other value"));
        }

        /// <summary>
        /// Retrieves a key that hasn't been added to a dictionary
        /// </summary>
        [TestMethod]
        public void RetrieveNonExistentKey()
        {
            var dictionary = new Dictionary<string, string>();
            Assert.IsFalse(dictionary.TryGetValue("some key", out var value));
        }
    }
}
