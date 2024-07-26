namespace Db.System.Collections.Generic
{
    using DbAdapters.System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
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

        [TestMethod]
        public void ToDictionarySelectorsNullValueSelector()
        {
            var enumerable = global::System.Linq.Enumerable.Empty<string>().ToDb();
            global::System.Func<string, string> valueSelector = null;
            Assert.ThrowsException<global::System.ArgumentNullException>(() => enumerable.ToDictionary(_ => _, valueSelector));
        }
    }
}
