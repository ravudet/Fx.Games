namespace Fx.Games.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Fx.Games.Game;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for <see cref="MonteCarloStrategy{TGame, TBoard, TMove, TPlayer}"/>
    /// </summary>
    [TestClass]
    public sealed class MonteCarloStrategyUnitTests
    {
        /// <summary>
        /// Creates a <see cref="MonteCarloStrategy{TGame, TBoard, TMove, TPlayer}"/> using a <see langword="null"/> settings
        /// </summary>
        [TestMethod]
        public void NullSettings()
        {
            var player = "player";
            var numberOfDecisions = 150;
            Assert.ThrowsException<ArgumentNullException>(() => new MonteCarloStrategy<NoImplementationGame, string[], string, string>(
                player,
                numberOfDecisions,
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                null
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                ));
        }

        /// <summary>
        /// Selects a move from a <see langword="null"/> game
        /// </summary>
        [TestMethod]
        public void SelectMoveNullGame()
        {
            var player = "player";
            var numberOfDecisions = 150;
            var strategy = new MonteCarloStrategy<NoImplementationGame, string[], string, string>(player, numberOfDecisions, MonteCarloStrategySettings<NoImplementationGame, string[], string, string>.Default);

            Assert.ThrowsException<ArgumentNullException>(() => strategy.SelectMove(
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                null
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                ));
        }

        /// <summary>
        /// Selects a move from a game with no moves
        /// </summary>
        [TestMethod]
        public void SelectMoveNoMoves()
        {
            var player = "player";
            var numberOfDecisions = 150;
            var strategy = new MonteCarloStrategy<MovelessGame, string[], string, string>(player, numberOfDecisions, MonteCarloStrategySettings<MovelessGame, string[], string, string>.Default);
            var game = new MovelessGame();

            Assert.ThrowsException<InvalidGameException>(() => strategy.SelectMove(game));
        }

        /// <summary>
        /// Selects a move from a game
        /// </summary>
        [TestMethod]
        public void SelectMove()
        {
            var player = "player";
            var numberOfDecisions = 150;
            var strategy = new MonteCarloStrategy<SingleMoveGame, string[], string, string>(player, numberOfDecisions, MonteCarloStrategySettings<SingleMoveGame, string[], string, string>.Default);
            var game = new SingleMoveGame(player, numberOfDecisions / 3);

            var move = strategy.SelectMove(game);

            Assert.AreEqual("asdf", move);
        }

        /// <summary>
        /// A <see cref="IGame{TGame, TBoard, TMove, TPlayer}"/> that has a single legal move
        /// </summary>
        private sealed class SingleMoveGame : IGame<SingleMoveGame, string[], string, string>
        {
            private readonly int numberOfMoves;

            private static int finishedGames = 0;

            public SingleMoveGame(string player, int numberOfMoves)
            {
                this.CurrentPlayer = player;
                this.numberOfMoves = numberOfMoves;
            }

            /// <summary>
            /// The single move that this game always has available
            /// </summary>
            private readonly IEnumerable<string> moves = Enumerable.Repeat("asdf", 1);

            /// <inheritdoc/>
            public string CurrentPlayer { get; }

            /// <inheritdoc/>
            public IEnumerable<string> Moves
            {
                get
                {
                    return this.moves;
                }
            }

            /// <inheritdoc/>
            public string[] Board => throw new NotImplementedException();

            /// <inheritdoc/>
            public WinnersAndLosers<string> WinnersAndLosers
            {
                get
                {
                    if (this.numberOfMoves == 0)
                    {
                        return new WinnersAndLosers<string>(
                            finishedGames % 3 == 0 ? new[] { this.CurrentPlayer } : Enumerable.Empty<string>(),
                            finishedGames % 3 == 1 ? new[] { this.CurrentPlayer } : Enumerable.Empty<string>(),
                            finishedGames % 3 == 2 ? new[] { this.CurrentPlayer } : Enumerable.Empty<string>());
                    }


                    return new WinnersAndLosers<string>(Enumerable.Empty<string>(), Enumerable.Empty<string>(), Enumerable.Empty<string>());
                }
            }

            /// <inheritdoc/>
            public bool IsGameOver
            {
                get
                {
                    if (this.numberOfMoves == 0)
                    {
                        ++finishedGames;
                        return true;
                    }

                    return false;
                }
            }

            /// <inheritdoc/>
            public SingleMoveGame CommitMove(string move)
            {
                return new SingleMoveGame(this.CurrentPlayer, this.numberOfMoves - 1);
            }
        }

        /// <summary>
        /// A <see cref="IGame{TGame, TBoard, TMove, TPlayer}"/> that never has any legal moves
        /// </summary>
        private sealed class MovelessGame : IGame<MovelessGame, string[], string, string>
        {
            /// <inheritdoc/>
            public string CurrentPlayer => throw new NotImplementedException();

            /// <inheritdoc/>
            public IEnumerable<string> Moves
            {
                get
                {
                    return Enumerable.Empty<string>();
                }
            }

            /// <inheritdoc/>
            public string[] Board => throw new NotImplementedException();

            /// <inheritdoc/>
            public WinnersAndLosers<string> WinnersAndLosers => throw new NotImplementedException();

            /// <inheritdoc/>
            public bool IsGameOver => throw new NotImplementedException();

            /// <inheritdoc/>
            public MovelessGame CommitMove(string move)
            {
                throw new NotImplementedException();
            }
        }
    }
}
