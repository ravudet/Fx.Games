namespace Fx.Games.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Fx.Games.Game;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for <see cref="ConsoleStrategy{TGame, TBoard, TMove, TPlayer}"/>
    /// </summary>
    [TestClass]
    public sealed class ConsoleStrategyUnitTests
    {
        /// <summary>
        /// Selects a move from a <see langword="null"/> game
        /// </summary>
        [TestMethod]
        public void SelectMoveNullGame()
        {
            var strategy = ConsoleStrategy<NoImplementationGame, string[], string, string>.Instance;

            Assert.ThrowsException<ArgumentNullException>(() => strategy.SelectMove(null));
        }

        /// <summary>
        /// Selects a move from a game with no moves
        /// </summary>
        [TestMethod]
        public void SelectMoveNoMoves()
        {
            var strategy = ConsoleStrategy<MovelessGame, string[], string, string>.Instance;
            var game = new MovelessGame();

            Assert.ThrowsException<InvalidGameException>(() => strategy.SelectMove(game));
        }

        /// <summary>
        /// Selects a move from a game
        /// </summary>
        [TestMethod]
        public void SelectMove()
        {
            var strategy = ConsoleStrategy<SingleMoveGame, string[], string, string>.Instance;
            var game = new SingleMoveGame();
            using (var textReader = new StringReader("0" + Environment.NewLine))
            {
                string move;
                lock (ConsoleUtilities.ConsoleLock)
                {
                    var oldStdIn = Console.In;
                    try
                    {
                        Console.SetIn(textReader);

                        move = strategy.SelectMove(game);
                    }
                    finally
                    {
                        Console.SetIn(oldStdIn);
                    }
                }

                Assert.AreEqual("asdf", move);
            }
        }

        /// <summary>
        /// Selects a move from a game that doesn't have that move
        /// </summary>
        [TestMethod]
        public void SelectMoveOutOfRange()
        {
            var strategy = ConsoleStrategy<SingleMoveGame, string[], string, string>.Instance;
            var game = new SingleMoveGame();
            using (var textReader = new StringReader("1" + Environment.NewLine + "0" + Environment.NewLine))
            {
                string move;
                lock (ConsoleUtilities.ConsoleLock)
                {
                    var oldStdIn = Console.In;
                    try
                    {
                        Console.SetIn(textReader);
                        move = strategy.SelectMove(game);
                    }
                    finally
                    {
                        Console.SetIn(oldStdIn);
                    }
                }

                Assert.AreEqual("asdf", move);
            }
        }

        /// <summary>
        /// Selects a move from a game when the move is not a valid index
        /// </summary>
        [TestMethod]
        public void SelectMoveNotAnIndex()
        {
            var strategy = ConsoleStrategy<SingleMoveGame, string[], string, string>.Instance;
            var game = new SingleMoveGame();
            using (var textReader = new StringReader("asdf" + Environment.NewLine + "1" + Environment.NewLine + "0" + Environment.NewLine))
            {
                string move;
                lock (ConsoleUtilities.ConsoleLock)
                {
                    var oldStdIn = Console.In;
                    try
                    {
                        Console.SetIn(textReader);

                        move = strategy.SelectMove(game);
                    }
                    finally
                    {
                        Console.SetIn(oldStdIn);
                    }
                }

                Assert.AreEqual("asdf", move);
            }
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
