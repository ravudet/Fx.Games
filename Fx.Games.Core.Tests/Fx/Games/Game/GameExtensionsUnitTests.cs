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

            //// TODO you have a model for idistribution in the /distribution branch; but, do you really want that? you won't be able to have a decision tree; you would still be able to have a monte carlo strategy; do you want to parameterize the tdistribution?
            //// TODO you *do* want the above; games like warhammer have continuous sets of states, so you want to be able to represent that; decision tree should probably require a selector from tdistribution to the weighted distribution (maybe just the portions?) that decision trees require
            ////
            //// TODO write decision tree using a tree abstract; do this first with commit move, then with exploremove
            //// TODO idistribution.sample(...) should take in a subsequent idistribution for composability
            //// TODO the game interface will need both `exploremove` *and* `commitmove` so that you don't leak hidden information to players
            //// TODO update weighted distribution to use "portions"
            //// TODO update decision tree to use weighted distribution (or a mapping from Tdistribution to portions?)
            //// TODO tdistribution should be covariant
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
