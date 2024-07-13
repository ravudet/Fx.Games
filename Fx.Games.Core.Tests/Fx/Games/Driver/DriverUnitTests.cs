namespace Fx.Games.Driver
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Fx.Games.Game;
    using Fx.Games.Strategy;
    using Microsoft.VisualStudio.TestPlatform;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for <see cref="Driver{TGame, TBoard, TMove, TPlayer}"/>
    /// </summary>
    [TestClass]
    public sealed class DriverUnitTests
    {
        /// <summary>
        /// Creates a driver with <see langword="null"/> strategies
        /// </summary>
        [TestMethod]
        public void NullStrategies()
        {
            //// TODO clean up 79e900e4a410cf85d445bc9b56edba6097887d10 onward
            /*var game = new MockGame();
            Driver.Create(
                new[] 
                { 
                    KeyValuePair.Create("first", MockStrategy.Create(game))
                }.ToCovariantReadOnlyDictionary(), 
                game.NullDisplayer());*/

            var game = new MockGame();
            Assert.ThrowsException<ArgumentNullException>(() => Driver.Create(
                null,
                game.NullDisplayer()));
        }

        /// <summary>
        /// Creates a driver with a <see langword="null"/> displayer
        /// </summary>
        [TestMethod]
        public void NullDisplayer()
        {
            var game = new MockGame();
            Assert.ThrowsException<ArgumentNullException>(() => Driver.Create(
                new[]
                {
                    KeyValuePair.Create("first", MockStrategy.Create(game)),
                }.ToCovariantReadOnlyDictionary(),
                null));
        }

        /// <summary>
        /// Runs a <see langword="null"/> game
        /// </summary>
        [TestMethod]
        public void NullGame()
        {
            var game = new MockGame();
            var driver = Driver.Create(
                new[]
                {
                    KeyValuePair.Create("first", MockStrategy.Create(game)),
                }.ToCovariantReadOnlyDictionary(),
                game.NullDisplayer());

            Assert.ThrowsException<ArgumentNullException>(() => driver.Run(null));
        }

        [TestMethod]
        public void PlayerNotFound()
        {
            var game = new MockGame();
            var driver = Driver.Create(
                new[]
                {
                    KeyValuePair.Create("first", MockStrategy.Create(game)),
                }.ToCovariantReadOnlyDictionary(),
                game.NullDisplayer());
            Assert.ThrowsException<PlayerNotFoundExeption>(() => driver.Run(game));
        }

        [TestMethod]
        public void StrategyCalled()
        {
            var game = new MockGame();
            var strategy = MockStrategy.Create(game);
            var driver = Driver.Create(
                new[]
                {
                    KeyValuePair.Create("asdf", strategy),
                }.ToCovariantReadOnlyDictionary(),
                game.NullDisplayer());
            driver.Run(game);

            var calledGames = strategy.CalledGames.ToList();
            Assert.AreEqual(1, calledGames.Count);
            Assert.AreEqual(game, calledGames[0]);
        }

        private sealed class MockGame : IGame<MockGame, string[], string, string>
        {
            public MockGame()
                : this(false)
            {
            }
            
            private MockGame(bool isGameOver)
            {
                this.IsGameOver = isGameOver;
            }

            public string CurrentPlayer => "asdf";

            public IEnumerable<string> Moves
            {
                get
                {
                    yield return "a move";
                }
            }

            public string[] Board => throw new System.NotImplementedException();

            public WinnersAndLosers<string> WinnersAndLosers => throw new System.NotImplementedException();

            public bool IsGameOver { get; }

            public MockGame CommitMove(string move)
            {
                return new MockGame(true);
            }
        }

        private static class MockStrategy
        {
            public static MockStrategy<TGame, TBoard, TMove, TPlayer> Create<TGame, TBoard, TMove, TPlayer>(IGame<TGame, TBoard, TMove, TPlayer> game) where TGame : IGame<TGame, TBoard, TMove, TPlayer>
            {
                return new MockStrategy<TGame, TBoard, TMove, TPlayer>();
            }
        }

        private sealed class MockStrategy<TGame, TBoard, TMove, TPlayer> : IStrategy<TGame, TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
        {
            private readonly List<TGame> calledGames;

            public MockStrategy()
            {
                this.calledGames = new List<TGame>();
            }

            public IEnumerable<TGame> CalledGames
            {
                get
                {
                    return this.calledGames;
                }
            }

            public TMove SelectMove(TGame game)
            {
                this.calledGames.Add(game);

                return game.Moves.First();
            }
        }
    }
}
