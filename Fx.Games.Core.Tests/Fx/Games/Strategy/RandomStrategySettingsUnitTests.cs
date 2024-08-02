namespace Fx.Games.Strategy
{
    using System;
    using System.Collections.Generic;

    using Fx.Games.Game;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for <see cref="RandomStrategySettings{TGame, TBoard, TMove, TPlayer}"/>
    /// </summary>
    [TestClass]
    public sealed class RandomStrategySettingsUnitTests
    {
        /// <summary>
        /// Creates default <see cref="RandomStrategySettings{TGame, TBoard, TMove, TPlayer}"/>
        /// </summary>
        [TestMethod]
        public void DefaultBuilder()
        {
            var builder = new RandomStrategySettings<NoImplementationGame, string[], string, string>.Builder()
            {
            };

            var settings = builder.Build();

            Assert.AreEqual(builder.Random, settings.Random);
        }

        /// <summary>
        /// Creates default <see cref="RandomStrategySettings{TGame, TBoard, TMove, TPlayer}"/>
        /// </summary>
        [TestMethod]
        public void DefaultSettings()
        {
            var builder = new RandomStrategySettings<NoImplementationGame, string[], string, string>.Builder()
            {
            };

            var settings = builder.Build();

            Assert.AreEqual(
                RandomStrategySettings<NoImplementationGame, string[], string, string>.Default.Random, 
                settings.Random);
        }

        /// <summary>
        /// Creates <see cref="RandomStrategySettings{TGame, TBoard, TMove, TPlayer}"/> with a <see langword="null"/> random
        /// </summary>
        [TestMethod]
        public void NullRandom()
        {
            var builder = new RandomStrategySettings<NoImplementationGame, string[], string, string>.Builder()
            {
                Random = null,
            };

            Assert.ThrowsException<ArgumentNullException>(() => builder.Build());
        }
    }
}
