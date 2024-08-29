namespace Fx.Games.Strategy
{
    using Fx.Games.Game;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    /// <summary>
    /// Unit tests for <see cref="MonteCarloStrategySettings{TGame, TBoard, TMove, TPlayer}"/>
    /// </summary>
    [TestClass]
    public sealed class MonteCarloStrategySettingsUnitTests
    {
        /// <summary>
        /// Creates default <see cref="MonteCarloStrategySettings{TGame, TBoard, TMove, TPlayer}"/>
        /// </summary>
        [TestMethod]
        public void DefaultBuilder()
        {
            var builder = new MonteCarloStrategySettings<NoImplementationGame, string[], string, string>.Builder()
            {
            };

            var settings = builder.Build();

            Assert.AreEqual(builder.PlayerComparer, settings.PlayerComparer);
            Assert.AreEqual(builder.Random, settings.Random);
        }

        /// <summary>
        /// Creates default <see cref="MonteCarloStrategySettings{TGame, TBoard, TMove, TPlayer}"/>
        /// </summary>
        [TestMethod]
        public void DefaultSettings()
        {
            var builder = new MonteCarloStrategySettings<NoImplementationGame, string[], string, string>.Builder()
            {
            };

            var settings = builder.Build();

            Assert.AreEqual(
                MonteCarloStrategySettings<NoImplementationGame, string[], string, string>.Default.PlayerComparer,
                settings.PlayerComparer);
            Assert.AreEqual(
                MonteCarloStrategySettings<NoImplementationGame, string[], string, string>.Default.Random, 
                settings.Random);
        }

        /// <summary>
        /// Creates <see cref="MonteCarloStrategySettings{TGame, TBoard, TMove, TPlayer}"/> with a <see langword="null"/> player comparer
        /// </summary>
        [TestMethod]
        public void NullPlayerComparer()
        {
            var builder = new MonteCarloStrategySettings<NoImplementationGame, string[], string, string>.Builder()
            {
                PlayerComparer =
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                    null,
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            };

            Assert.ThrowsException<ArgumentNullException>(() => builder.Build());
        }

        /// <summary>
        /// Creates <see cref="MonteCarloStrategySettings{TGame, TBoard, TMove, TPlayer}"/> with a <see langword="null"/> random
        /// </summary>
        [TestMethod]
        public void NullRandom()
        {
            var builder = new MonteCarloStrategySettings<NoImplementationGame, string[], string, string>.Builder()
            {
                Random =
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                    null,
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            };

            Assert.ThrowsException<ArgumentNullException>(() => builder.Build());
        }
    }
}
