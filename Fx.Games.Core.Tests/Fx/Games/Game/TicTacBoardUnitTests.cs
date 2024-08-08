namespace Fx.Games.Game
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for <see cref="TicTacToeBoard"/>
    /// </summary>
    [TestClass]
    public sealed class TicTacBoardUnitTests
    {
        /// <summary>
        /// Creates a tic-tac-toe board with a <see langword="null"/> grid
        /// </summary>
        [TestMethod]
        public void NullGrid()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new TicTacToeBoard(
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                // SUPPRESSION test case for the null validation
                null
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                ));
        }

        /// <summary>
        /// Creates a tic-tac-toe board with a 4 by 3 grid
        /// </summary>
        [TestMethod]
        public void FourByThreeGrid()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new TicTacToeBoard(new TicTacToePiece[4, 3]));
        }

        /// <summary>
        /// Creates a tic-tac-toe board with a 3 by 4 grid
        /// </summary>
        [TestMethod]
        public void ThreeByFourGrid()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new TicTacToeBoard(new TicTacToePiece[3, 4]));
        }

        /// <summary>
        /// Creates an empty tic-tac-toe board
        /// </summary>
        [TestMethod]
        public void EmptyBoard()
        {
            var board = new TicTacToeBoard();

            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    Assert.AreEqual(TicTacToePiece.Empty, board.Grid[i, j]);
                }
            }
        }

        /// <summary>
        /// Mutates the grid parameter of the tic-tac-toe board
        /// </summary>
        [TestMethod]
        public void ClonedGrid()
        {
            var grid = new TicTacToePiece[3, 3];
            var board = new TicTacToeBoard(grid);

            grid[0, 0] = TicTacToePiece.Oh;

            Assert.AreEqual(TicTacToePiece.Empty, board.Grid[0, 0]);
        }

        /// <summary>
        /// Mutates the grid of a tic-tac-toe board
        /// </summary>
        [TestMethod]
        public void MutatedGrid()
        {
            var board = new TicTacToeBoard();
            Assert.AreEqual(TicTacToePiece.Empty, board.Grid[0, 0]);

            board.Grid[0, 0] = TicTacToePiece.Oh;

            Assert.AreEqual(TicTacToePiece.Empty, board.Grid[0, 0]);
        }
    }
}
