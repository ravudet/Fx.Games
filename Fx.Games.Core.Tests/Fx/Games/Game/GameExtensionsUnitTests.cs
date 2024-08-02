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
    }
}
