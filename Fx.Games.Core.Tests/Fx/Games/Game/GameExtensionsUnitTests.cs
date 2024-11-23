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

            //// TODO ConvertToPortion doesn't work
            //// TODO you are dealing with "portions" because they should let you use a weighted distribution
            //// TODO you have a model for idistribution in the /distribution branch; but, do you really want that? you won't be able to have a decision tree; you would still be able to have a monte carlo strategy; do you want to parameterize the tdistribution?
            //// TODO idistribution.sample(...) should take in a subsequent idistribution for composability
            var convertedPortion = PortionV2Playground.ConvertToPortion(weights);

            var convertedWeights = PortionV2Playground.ConvertToWeights(convertedPortion);
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
