namespace Fx.Games.Game
{
    using System;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for <see cref="WinnersAndLosers{TPlayer}"/>
    /// </summary>
    [TestClass]
    public sealed class WinnersAndLosersUnitTests
    {
        /// <summary>
        /// Creates a <see cref="WinnersAndLosers{TPlayer}"/> with <see langword="null"/> winners
        /// </summary>
        [TestMethod]
        public void NullWinners()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new WinnersAndLosers<string>(
                null,
                Enumerable.Empty<string>(),
                Enumerable.Empty<string>()));
        }

        /// <summary>
        /// Creates a <see cref="WinnersAndLosers{TPlayer}"/> with <see langword="null"/> losers
        /// </summary>
        [TestMethod]
        public void NullLosers()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new WinnersAndLosers<string>(
                Enumerable.Empty<string>(),
                null,
                Enumerable.Empty<string>()));
        }

        /// <summary>
        /// Creates a <see cref="WinnersAndLosers{TPlayer}"/> with <see langword="null"/> drawers
        /// </summary>
        [TestMethod]
        public void NullDrawers()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new WinnersAndLosers<string>(
                Enumerable.Empty<string>(),
                Enumerable.Empty<string>(),
                null));
        }
    }
}
