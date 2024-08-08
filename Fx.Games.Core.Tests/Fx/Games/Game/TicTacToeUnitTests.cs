namespace Fx.Games.Game
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Linq;

    /// <summary>
    /// Unit tests for <see cref="TicTacToe{TPlayer}"/>
    /// </summary>
    [TestClass]
    public sealed class TicTacToeUnitTests
    {
        /// <summary>
        /// Creates a new tic-tac-toe game with a <see langword="null"/> 'X' player
        /// </summary>
        [TestMethod]
        public void NullEx()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new TicTacToe<string>(
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                // SUPPRESSION test case for the null validation
                null,
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                "ohs"));
        }

        /// <summary>
        /// Creates a new tic-tac-toe game with a <see langword="null"/> 'O' player
        /// </summary>
        [TestMethod]
        public void NullOh()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new TicTacToe<string>(
                "exes",
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                // SUPPRESSION test case for the null validation
                null
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                ));
        }

        /// <summary>
        /// Commits a <see langword="null"/> move
        /// </summary>
        [TestMethod]
        public void CommitNullMove()
        {
            var ticTacToe = new TicTacToe<string>("exes", "ohs");

            Assert.ThrowsException<ArgumentNullException>(() => ticTacToe.CommitMove(
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                // SUPPRESSION test case for the null validation
                null
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                ));
        }

        /// <summary>
        /// Commits an illegal move
        /// </summary>
        [TestMethod]
        public void CommitIllegalMove()
        {
            var ticTacToe = new TicTacToe<string>("exes", "ohs");

            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(0, 0));

            Assert.ThrowsException<IllegalMoveExeption>(() => ticTacToe.CommitMove(new TicTacToeMove(0, 0)));
        }

        /// <summary>
        /// Plays a game where exes win a row
        /// </summary>
        [TestMethod]
        public void ExesWinRow()
        {
            var ticTacToe = new TicTacToe<string>("exes", "ohs");

            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(0, 0));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(1, 0));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(0, 1));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(1, 1));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(0, 2));

            Assert.IsTrue(ticTacToe.IsGameOver);
            CollectionAssert.AreEqual(new[] { "exes" }, ticTacToe.WinnersAndLosers.Winners.ToArray());
            CollectionAssert.AreEqual(new[] { "ohs" }, ticTacToe.WinnersAndLosers.Losers.ToArray());
            Assert.IsFalse(ticTacToe.WinnersAndLosers.Drawers.Any());
        }

        /// <summary>
        /// Plays a game where exes win a column
        /// </summary>
        [TestMethod]
        public void ExesWinColumn()
        {
            var ticTacToe = new TicTacToe<string>("exes", "ohs");

            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(0, 0));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(0, 1));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(1, 0));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(1, 1));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(2, 0));

            Assert.IsTrue(ticTacToe.IsGameOver);
            CollectionAssert.AreEqual(new[] { "exes" }, ticTacToe.WinnersAndLosers.Winners.ToArray());
            CollectionAssert.AreEqual(new[] { "ohs" }, ticTacToe.WinnersAndLosers.Losers.ToArray());
            Assert.IsFalse(ticTacToe.WinnersAndLosers.Drawers.Any());
        }

        /// <summary>
        /// Plays a game where exes win the top-left diagonal
        /// </summary>
        [TestMethod]
        public void ExesWinTopLeft()
        {
            var ticTacToe = new TicTacToe<string>("exes", "ohs");

            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(0, 0));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(0, 1));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(1, 1));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(0, 2));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(2, 2));

            Assert.IsTrue(ticTacToe.IsGameOver);
            CollectionAssert.AreEqual(new[] { "exes" }, ticTacToe.WinnersAndLosers.Winners.ToArray());
            CollectionAssert.AreEqual(new[] { "ohs" }, ticTacToe.WinnersAndLosers.Losers.ToArray());
            Assert.IsFalse(ticTacToe.WinnersAndLosers.Drawers.Any());
        }

        /// <summary>
        /// Plays a game where exes win the bottom-left diagonal
        /// </summary>
        [TestMethod]
        public void ExesWinBottomLeft()
        {
            var ticTacToe = new TicTacToe<string>("exes", "ohs");

            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(2, 0));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(0, 1));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(1, 1));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(1, 0));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(0, 2));

            Assert.IsTrue(ticTacToe.IsGameOver);
            CollectionAssert.AreEqual(new[] { "exes" }, ticTacToe.WinnersAndLosers.Winners.ToArray());
            CollectionAssert.AreEqual(new[] { "ohs" }, ticTacToe.WinnersAndLosers.Losers.ToArray());
            Assert.IsFalse(ticTacToe.WinnersAndLosers.Drawers.Any());
        }

        /// <summary>
        /// Plays a game where ohs win a row
        /// </summary>
        [TestMethod]
        public void OhsWinRow()
        {
            var ticTacToe = new TicTacToe<string>("exes", "ohs");

            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(0, 0));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(1, 0));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(0, 1));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(1, 1));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(2, 0));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(1, 2));

            Assert.IsTrue(ticTacToe.IsGameOver);
            CollectionAssert.AreEqual(new[] { "ohs" }, ticTacToe.WinnersAndLosers.Winners.ToArray());
            CollectionAssert.AreEqual(new[] { "exes" }, ticTacToe.WinnersAndLosers.Losers.ToArray());
            Assert.IsFalse(ticTacToe.WinnersAndLosers.Drawers.Any());
        }

        /// <summary>
        /// Plays a game where exes ohs a column
        /// </summary>
        [TestMethod]
        public void OhsWinColumn()
        {
            var ticTacToe = new TicTacToe<string>("exes", "ohs");

            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(0, 0));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(0, 1));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(1, 0));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(1, 1));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(0, 2));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(2, 1));

            Assert.IsTrue(ticTacToe.IsGameOver);
            CollectionAssert.AreEqual(new[] { "ohs" }, ticTacToe.WinnersAndLosers.Winners.ToArray());
            CollectionAssert.AreEqual(new[] { "exes" }, ticTacToe.WinnersAndLosers.Losers.ToArray());
            Assert.IsFalse(ticTacToe.WinnersAndLosers.Drawers.Any());
        }

        /// <summary>
        /// Plays a game where ohs win the top-left diagonal
        /// </summary>
        [TestMethod]
        public void OhsWinTopLeft()
        {
            var ticTacToe = new TicTacToe<string>("exes", "ohs");

            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(1, 0));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(0, 0));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(2, 0));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(1, 1));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(1, 2));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(2, 2));

            Assert.IsTrue(ticTacToe.IsGameOver);
            CollectionAssert.AreEqual(new[] { "ohs" }, ticTacToe.WinnersAndLosers.Winners.ToArray());
            CollectionAssert.AreEqual(new[] { "exes" }, ticTacToe.WinnersAndLosers.Losers.ToArray());
            Assert.IsFalse(ticTacToe.WinnersAndLosers.Drawers.Any());
        }

        /// <summary>
        /// Plays a game where ohs win the bottom-left diagonal
        /// </summary>
        [TestMethod]
        public void OhsWinBottomLeft()
        {
            var ticTacToe = new TicTacToe<string>("exes", "ohs");

            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(1, 0));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(2, 0));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(0, 1));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(1, 1));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(1, 2));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(0, 2));

            Assert.IsTrue(ticTacToe.IsGameOver);
            CollectionAssert.AreEqual(new[] { "ohs" }, ticTacToe.WinnersAndLosers.Winners.ToArray());
            CollectionAssert.AreEqual(new[] { "exes" }, ticTacToe.WinnersAndLosers.Losers.ToArray());
            Assert.IsFalse(ticTacToe.WinnersAndLosers.Drawers.Any());
        }

        /// <summary>
        /// Plays a game that ends in a draw
        /// </summary>
        [TestMethod]
        public void DrawnGame()
        {
            var ticTacToe = new TicTacToe<string>("exes", "ohs");

            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(0, 0));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(1, 1));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(2, 0));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(1, 0));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(1, 2));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(0, 1));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(2, 1));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(2, 2));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(0, 2));

            Assert.IsTrue(ticTacToe.IsGameOver);
            Assert.IsFalse(ticTacToe.WinnersAndLosers.Winners.Any());
            Assert.IsFalse(ticTacToe.WinnersAndLosers.Losers.Any());
            CollectionAssert.AreEqual(new[] { "exes", "ohs" }, ticTacToe.WinnersAndLosers.Drawers.ToArray());
        }

        /// <summary>
        /// Plays tic-tac-toe always committing the first move
        /// </summary>
        [TestMethod]
        public void FirstMoves()
        {
            var ticTacToe = new TicTacToe<string>("exes", "ohs");

            Assert.IsFalse(ticTacToe.IsGameOver);
            Assert.AreEqual("exes", ticTacToe.CurrentPlayer);
            var emptySquareCount = EmptySquareCount(ticTacToe);
            Assert.AreEqual(9, emptySquareCount);

            while (!ticTacToe.IsGameOver)
            {
                Assert.IsFalse(ticTacToe.WinnersAndLosers.Winners.Any());
                Assert.IsFalse(ticTacToe.WinnersAndLosers.Losers.Any());
                Assert.IsFalse(ticTacToe.WinnersAndLosers.Drawers.Any());

                ticTacToe = ticTacToe.CommitMove(ticTacToe.Moves.First());

                Assert.AreEqual(--emptySquareCount, EmptySquareCount(ticTacToe));
            }
        }

        /// <summary>
        /// Plays tic-tac-toe always committing the last move
        /// </summary>
        [TestMethod]
        public void LastMoves()
        {
            var ticTacToe = new TicTacToe<string>("exes", "ohs");

            Assert.IsFalse(ticTacToe.IsGameOver);
            Assert.AreEqual("exes", ticTacToe.CurrentPlayer);
            var emptySquareCount = EmptySquareCount(ticTacToe);
            Assert.AreEqual(9, emptySquareCount);

            while (!ticTacToe.IsGameOver)
            {
                Assert.IsFalse(ticTacToe.WinnersAndLosers.Winners.Any());
                Assert.IsFalse(ticTacToe.WinnersAndLosers.Losers.Any());
                Assert.IsFalse(ticTacToe.WinnersAndLosers.Drawers.Any());

                ticTacToe = ticTacToe.CommitMove(ticTacToe.Moves.Last());

                Assert.AreEqual(--emptySquareCount, EmptySquareCount(ticTacToe));
            }
        }

        private static int EmptySquareCount<TPlayer>(TicTacToe<TPlayer> ticTacToe)
        {
            var grid = ticTacToe.Board.Grid;

            var count = 0;
            for (int i = 0; i < grid.GetLength(0); ++i)
            {
                for (int j = 0; j < grid.GetLength(1); ++j)
                {
                    if (grid[i, j] == TicTacToePiece.Empty)
                    {
                        ++count;
                    }
                }
            }

            return count;
        }
    }
}
