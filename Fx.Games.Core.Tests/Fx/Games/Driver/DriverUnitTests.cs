namespace Fx.Games.Driver
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Db.System.Collections.Generic;
    using DbAdapters.System.Collections.Generic;
    using Fx.Games.Game;
    using Fx.Games.Strategy;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for <see cref="Driver{TGame, TBoard, TMove, TPlayer}"/> and <see cref="Driver"/>
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
            var game = new MockGame();
            Assert.ThrowsException<ArgumentNullException>(() => new Driver<MockGame, string[], string, string>(
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                // SUPPRESSION test case for the null validation
                null,
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                game.NullDisplayer()));
        }

        /// <summary>
        /// Creates a driver with a <see langword="null"/> displayer
        /// </summary>
        [TestMethod]
        public void NullDisplayer()
        {
            var game = new MockGame();
            Assert.ThrowsException<ArgumentNullException>(() => new Driver<MockGame, string[], string, string>(
                new[]
                {
                    KeyValuePair.Create("first", MockStrategy.Create(game)),
                }.ToDb().ToDictionary(),
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                // SUPPRESSION test case for the null validation
                null
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                ));
        }

        /// <summary>
        /// Creates a driver with <see langword="null"/> settings
        /// </summary>
        [TestMethod]
        public void NullSettings()
        {
            var game = new MockGame();
            Assert.ThrowsException<ArgumentNullException>(() => new Driver<MockGame, string[], string, string>(
                new[]
                {
                    KeyValuePair.Create("first", MockStrategy.Create(game)),
                }.ToDb().ToDictionary(),
                game.NullDisplayer(),
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                // SUPPRESSION test case for the null validation
                null
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                ));
        }

        /// <summary>
        /// Creates a driver with <see langword="null"/> strategies
        /// </summary>
        [TestMethod]
        public void NullStrategiesFactory()
        {
            var game = new MockGame();
            Assert.ThrowsException<ArgumentNullException>(() => Driver.Create(
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                // SUPPRESSION test case for the null validation
                null,
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                game.NullDisplayer()));
        }

        /// <summary>
        /// Creates a driver with a <see langword="null"/> displayer
        /// </summary>
        [TestMethod]
        public void NullDisplayerFactory()
        {
            var game = new MockGame();
            Assert.ThrowsException<ArgumentNullException>(() => Driver.Create(
                new[]
                {
                    KeyValuePair.Create("first", MockStrategy.Create(game)),
                }.ToDb().ToDictionary(),
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                // SUPPRESSION test case for the null validation
                null
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                ));
        }

        /// <summary>
        /// Creates a driver with <see langword="null"/> settings
        /// </summary>
        [TestMethod]
        public void NullSettingsFactory()
        {
            var game = new MockGame();
            Assert.ThrowsException<ArgumentNullException>(() => Driver.Create(
                new[]
                {
                    KeyValuePair.Create("first", MockStrategy.Create(game)),
                }.ToDb().ToDictionary(),
                game.NullDisplayer(),
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                // SUPPRESSION test case for the null validation
                null
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                ));
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
                }.ToDb().ToDictionary(),
                game.NullDisplayer());

            Assert.ThrowsException<ArgumentNullException>(() => driver.Run(
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                // SUPPRESSION test case for the null validation
                null
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                ));
        }

        /// <summary>
        /// Runs a game that has a player who does not have a strategy configured in the driver
        /// </summary>
        [TestMethod]
        public void PlayerNotFound()
        {
            var game = new MockGame();
            var driver = Driver.Create(
                new[]
                {
                    KeyValuePair.Create("first", MockStrategy.Create(game)),
                }.ToDb().ToDictionary(),
                game.NullDisplayer());
            Assert.ThrowsException<PlayerNotFoundExeption>(() => driver.Run(game));
        }

        /// <summary>
        /// Runs a game that has a player who does not have a strategy configured in the driver
        /// </summary>
        [TestMethod]
        public void PlayerNotFoundTranscriber()
        {
            var game = new MockGame();
            var driverSettings = game.DriverSettings();
            driverSettings.PlayerTranscriber = player => nameof(PlayerNotFoundTranscriber);
            var driver = Driver.Create(
                new[]
                {
                    KeyValuePair.Create("first", MockStrategy.Create(game)),
                }.ToDb().ToDictionary(),
                game.NullDisplayer(),
                driverSettings.Build());
            var playerNotFoundException = Assert.ThrowsException<PlayerNotFoundExeption>(() => driver.Run(game));
        }

        /// <summary>
        /// Runs a game where strategies are actually leveraged to progress the game
        /// </summary>
        [TestMethod]
        public void StrategyCalled()
        {
            var game = new MockGame();
            var strategy = MockStrategy.Create(game);
            var driver = Driver.Create(
                new[]
                {
                    KeyValuePair.Create("asdf", strategy),
                }.ToDb().ToDictionary(),
                game.NullDisplayer());
            driver.Run(game);

            var calledGames = strategy.CalledGames.ToList();
            Assert.AreEqual(1, calledGames.Count);
            Assert.AreEqual(game, calledGames[0]);
        }

        /// <summary>
        /// A mock implementation of <see cref="IGame{TGame, TBoard, TMove, TPlayer}"/> that plays a single turn with a single move
        /// </summary>
        private sealed class MockGame : IGame<MockGame, string[], string, string>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="MockGame"/> class
            /// </summary>
            public MockGame()
                : this(false)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="MockGame"/> class
            /// </summary>
            /// <param name="isGameOver">Whether the game should be considered over or not</param>
            private MockGame(bool isGameOver)
            {
                this.IsGameOver = isGameOver;
            }

            /// <inheritdoc/>
            public string CurrentPlayer => "asdf";

            /// <inheritdoc/>
            public global::System.Collections.Generic.IEnumerable<string> Moves
            {
                get
                {
                    yield return "a move";
                }
            }

            /// <inheritdoc/>
            public string[] Board => throw new System.NotImplementedException();

            /// <inheritdoc/>
            public WinnersAndLosers<string> WinnersAndLosers => throw new System.NotImplementedException();

            /// <inheritdoc/>
            public bool IsGameOver { get; }

            /// <inheritdoc/>
            public MockGame CommitMove(string move)
            {
                return new MockGame(true);
            }
        }

        /// <summary>
        /// Factories for <see cref="MockStrategy{TGame, TBoard, TMove, TPlayer}"/>
        /// </summary>
        private static class MockStrategy
        {
            /// <summary>
            /// Creates a new instance of <see cref="MockStrategy{TGame, TBoard, TMove, TPlayer}"/>
            /// </summary>
            /// <typeparam name="TGame">The type of the game that the new <see cref="MockStrategy{TGame, TBoard, TMove, TPlayer}"/> will play</typeparam>
            /// <typeparam name="TBoard">The type of the board that the <typeparamref name="TGame"/> uses</typeparam>
            /// <typeparam name="TMove">The type of the moves that the <typeparamref name="TGame"/> uses</typeparam>
            /// <typeparam name="TPlayer">The type of the player that is playing the <typeparamref name="TGame"/></typeparam>
            /// <param name="game">The game that the new <see cref="MockStrategy{TGame, TBoard, TMove, TPlayer}"/> will play</param>
            /// <returns>A new instance of <see cref="MockStrategy{TGame, TBoard, TMove, TPlayer}"/></returns>
            public static MockStrategy<TGame, TBoard, TMove, TPlayer> Create<TGame, TBoard, TMove, TPlayer>(IGame<TGame, TBoard, TMove, TPlayer> game) where TGame : IGame<TGame, TBoard, TMove, TPlayer>
            {
                return new MockStrategy<TGame, TBoard, TMove, TPlayer>();
            }
        }

        /// <summary>
        /// A mock implementation of <see cref="IStrategy{TGame, TBoard, TMove, TPlayer}"/> that always selects the first available move and keeps track of the games that it was asked to select a move for
        /// </summary>
        /// <typeparam name="TGame">The type of the game that the strategy can play</typeparam>
        /// <typeparam name="TBoard">The type of the board that the <typeparamref name="TGame"/> uses</typeparam>
        /// <typeparam name="TMove">The type of the moves that the <typeparamref name="TGame"/> uses</typeparam>
        /// <typeparam name="TPlayer">The type of the player that is playing the <typeparamref name="TGame"/></typeparam>
        private sealed class MockStrategy<TGame, TBoard, TMove, TPlayer> : IStrategy<TGame, TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
        {
            /// <summary>
            /// The games that this strategy has been asked to select a move for
            /// </summary>
            private readonly List<TGame> calledGames;

            /// <summary>
            /// Initializes a new instance of the <see cref="MockStrategy{TGame, TBoard, TMove, TPlayer}"/> class
            /// </summary>
            public MockStrategy()
            {
                this.calledGames = new List<TGame>();
            }

            /// <summary>
            /// The games that this strategy has been asked to select a move for
            /// </summary>
            public global::System.Collections.Generic.IEnumerable<TGame> CalledGames
            {
                get
                {
                    foreach (var element in this.calledGames)
                    {
                        yield return element;
                    }
                }
            }

            /// <inheritdoc/>
            public TMove SelectMove(TGame game)
            {
                this.calledGames.Add(game);

                return game.Moves.First();
            }
        }
    }
}
