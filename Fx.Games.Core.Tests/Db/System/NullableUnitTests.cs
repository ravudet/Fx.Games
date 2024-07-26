namespace Db.System
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    /// <summary>
    /// Unit tests for <see cref="Nullable{T}"/>
    /// </summary>
    [TestClass]
    public sealed class NullableUnitTests
    {
        /// <summary>
        /// Gets the value of an uninitialized nullable
        /// </summary>
        [TestMethod]
        public void GetValueUnitialized()
        {
            var nullable = new Nullable<string>();

            Assert.AreEqual(false, nullable.HasValue);
            Assert.ThrowsException<global::System.InvalidOperationException>(() => nullable.Value);
        }

        /// <summary>
        /// Gets the value of an initialized nullable
        /// </summary>
        [TestMethod]
        public void GetValueInitialized()
        {
            var nullable = new Nullable<string>("asdf");

            Assert.AreEqual(true, nullable.HasValue);
            Assert.AreEqual("asdf", nullable.Value);
        }
    }
}
