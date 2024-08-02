namespace Fx.Games.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Fx.Games.Game;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for <see cref="RandomStrategy{TGame, TBoard, TMove, TPlayer}"/>
    /// </summary>
    [TestClass]
    public sealed class RandomStrategyUnitTests
    {
        /// <summary>
        /// Creates a <see cref="RandomStrategy{TGame, TBoard, TMove, TPlayer}"/> using <see langword="null"/> settings
        /// </summary>
        [TestMethod]
        public void NullSettings()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new RandomStrategy<NoImplementationGame, string[], string, string>(null));
        }

        /// <summary>
        /// Selects a move from a <see langword="null"/> game
        /// </summary>
        [TestMethod]
        public void SelectMoveNullGame()
        {
            var strategy = new RandomStrategy<NoImplementationGame, string[], string, string>();

            Assert.ThrowsException<ArgumentNullException>(() => strategy.SelectMove(null));
        }

        /// <summary>
        /// Selects a move from a game with no moves
        /// </summary>
        [TestMethod]
        public void SelectMoveNoMoves()
        {
            var strategy = new RandomStrategy<MovelessGame, string[], string, string>();
            var game = new MovelessGame();

            Assert.ThrowsException<InvalidGameException>(() => strategy.SelectMove(game));
        }

        /// <summary>
        /// Selects a move from a game
        /// </summary>
        [TestMethod]
        public void SelectMove()
        {
            var strategy = new RandomStrategy<SingleMoveGame, string[], string, string>();
            var game = new SingleMoveGame();

            var move = strategy.SelectMove(game);

            Assert.AreEqual("asdf", move);
        }

        /// <summary>
        /// A <see cref="IGame{TGame, TBoard, TMove, TPlayer}"/> that has a single legal move
        /// </summary>
        private sealed class SingleMoveGame : IGame<SingleMoveGame, string[], string, string>
        {
            /// <summary>
            /// The single move that this game always has available
            /// </summary>
            private readonly IEnumerable<string> moves = Enumerable.Repeat("asdf", 1);

            /// <inheritdoc/>
            public string CurrentPlayer => throw new NotImplementedException();

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
            public WinnersAndLosers<string> WinnersAndLosers => throw new NotImplementedException();

            /// <inheritdoc/>
            public bool IsGameOver => throw new NotImplementedException();

            /// <inheritdoc/>
            public SingleMoveGame CommitMove(string move)
            {
                throw new NotImplementedException();
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
