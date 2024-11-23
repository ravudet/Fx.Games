namespace Fx.Games.Game
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for <see cref="GameExtensions"/>
    /// </summary>
    [TestClass]
    public sealed class GameExtensionsUnitTests
    {
        [TestMethod]
        public void Portion()
        {
            var portion = PortionV2.Some(
                "ASdf", uint.MaxValue >> 1,
                PortionV2.Some(
                    "qwer", uint.MaxValue >> 1,
                    PortionV2.Some(
                        "1234", uint.MaxValue >> 1,
                        PortionV2.All("zxcv"))));

            var weights = PortionV2Playground.ConvertToWeights(portion);
        }

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
