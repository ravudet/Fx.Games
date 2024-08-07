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
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                // SUPPRESSION test case for the null validation
                null,
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
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
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                // SUPPRESSION test case for the null validation
                null,
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
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
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                // SUPPRESSION test case for the null validation
                null
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                ));
        }

        /// <summary>
        /// Creates a <see cref="WinnersAndLosers{TPlayer}"/> with <see langword="null"/> drawers
        /// </summary>
        [TestMethod]
        public void WinnersAndLosersData()
        {
            var winnersAndLosers = new WinnersAndLosers<string>(
                new[] { "winner" },
                new[] { "loser" },
                new[] { "drawer" });

            CollectionAssert.AreEqual(new[] { "winner" }, winnersAndLosers.Winners.ToList());
            CollectionAssert.AreEqual(new[] { "loser" }, winnersAndLosers.Losers.ToList());
            CollectionAssert.AreEqual(new[] { "drawer" }, winnersAndLosers.Drawers.ToList());
        }
    }
}
