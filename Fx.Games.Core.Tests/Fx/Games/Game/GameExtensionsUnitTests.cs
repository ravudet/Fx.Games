namespace Fx.Games.Game
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for <see cref="GameExtensions"/>
    /// </summary>
    [TestClass]
    public sealed class GameExtensionsUnitTests
    {
        /// <summary>
        /// Gets the random strategy for a game
        /// </summary>
        [TestMethod]
        public void RandomStrategy()
        {
            var strategy = NoImplementationGame.Instance.RandomStrategy();

            Assert.IsNotNull(strategy);
        }

        /// <summary>
        /// Gets the console strategy for a game
        /// </summary>
        [TestMethod]
        public void ConsoleStrategy()
        {
            var strategy = NoImplementationGame.Instance.ConsoleStrategy();

            Assert.IsNotNull(strategy);
        }

        /// <summary>
        /// Gets the monte carlo strategy for a game
        /// </summary>
        [TestMethod]
        public void MonteCarloStrategy()
        {
            var strategy = NoImplementationGame.Instance.MonteCarloStrategy("player", 100, NoImplementationGame.Instance.MonteCarloStrategySettings());

            Assert.IsNotNull(strategy);
        }
    }
}
