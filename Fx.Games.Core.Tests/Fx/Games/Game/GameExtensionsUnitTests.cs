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

        [TestMethod]
        public void Portion2()
        {
            var portion = PortionV2.Some(
                "ASdf", uint.MaxValue >> 1,
                PortionV2.Some(
                    "qwer", uint.MaxValue >> 1,
                    PortionV2.Some(
                        "1234", uint.MaxValue >> 1,
                        PortionV2.All("zxcv"))));

            var weights = PortionV2Playground.ConvertToWeights(portion);

            //// TODO use t4 (or something) to generate more naturals
            //// TODO update weighted distribution to use "portions"
            //// TODO tdistribution should be covariant
            //// TODO update monte carlo (and all other strategies) to use exploremove
            //// TODO implement battleship
            var convertedPortion = PortionV2Playground.ConvertToPortion(weights);

            var convertedWeights = PortionV2Playground.ConvertToWeights(convertedPortion);
        }

        [TestMethod]
        public void Portion3()
        {
            var weights = new[]
            {
                ((double)1 / 6, "asdf"),
                ((double)1 / 5, "qwer"),
                ((double)3 / 20, "1234"),
                ((double)29 / 60, "zxcv"),
            };
            var portions = PortionV2Playground.ConvertToPortion(weights);

            var convertedWeights = PortionV2Playground.ConvertToWeights(portions);
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
