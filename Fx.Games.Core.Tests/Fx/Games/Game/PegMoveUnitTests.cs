﻿namespace Fx.Games.Game
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for <see cref="PegMove"/>
    /// </summary>
    [TestClass]
    public sealed class PegMoveUnitTests
    {
        /// <summary>
        /// Creates a peg move with a <see langword="null"/> start
        /// </summary>
        [TestMethod]
        public void NullStart()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new PegMove(null, new PegPosition(3, 2)));
        }

        /// <summary>
        /// Creates a peg move with a <see langword="null"/> end
        /// </summary>
        [TestMethod]
        public void NullEnd()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new PegMove(new PegPosition(3, 2), null));
        }

        /// <summary>
        /// Creates a peg move
        /// </summary>
        [TestMethod]
        public void MovePeg()
        {
            var pegMove = new PegMove(new PegPosition(3, 2), new PegPosition(4, 2));

            Assert.AreEqual(3, pegMove.Start.Row);
            Assert.AreEqual(2, pegMove.Start.Column);
            Assert.AreEqual(4, pegMove.End.Row);
            Assert.AreEqual(2, pegMove.End.Column);
        }
    }
}
