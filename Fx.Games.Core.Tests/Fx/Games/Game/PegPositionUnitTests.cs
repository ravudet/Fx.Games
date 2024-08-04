namespace Fx.Games.Game
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    /// <summary>
    /// Unit tests for <see cref="PegPosition"/>
    /// </summary>
    [TestClass]
    public sealed class PegPositionUnitTests
    {
        /// <summary>
        /// Creates a peg position with a negative row
        /// </summary>
        [TestMethod]
        public void NegativeRow()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new PegPosition(-1, 3));
        }

        /// <summary>
        /// Creates a peg position with a row beyond the triangle
        /// </summary>
        [TestMethod]
        public void LargeRow()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new PegPosition(5, 3));
        }

        /// <summary>
        /// Creates a peg position with a negative column
        /// </summary>
        [TestMethod]
        public void NegativeColumn()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new PegPosition(0, -1));
        }

        /// <summary>
        /// Creates a peg position with a column beyond the triangle
        /// </summary>
        [TestMethod]
        public void LargeColumn()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new PegPosition(3, 4));
        }

        /// <summary>
        /// Creates a peg position that is within the triangle
        /// </summary>
        [TestMethod]
        public void WithinTriangle()
        {
            var pegPosition = new PegPosition(3, 2);

            Assert.AreEqual(3, pegPosition.Row);
            Assert.AreEqual(2, pegPosition.Column);
        }
    }
}
