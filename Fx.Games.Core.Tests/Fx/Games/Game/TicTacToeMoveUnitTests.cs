namespace Fx.Games.Game
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;

    /// <summary>
    /// Unit tests for <see cref="TicTacToeMove"/>
    /// </summary>
    [TestClass]
    public sealed class TicTacToeMoveUnitTests
    {
        /// <summary>
        /// Creates a tic-tac-toe move with a row that is out-of-bounds of the grid
        /// </summary>
        [TestMethod]
        public void LargeRow()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new TicTacToeMove(3, 0));
        }

        /// <summary>
        /// Creates a tic-tac-toe move with a column that is out-of-bounds of the grid
        /// </summary>
        [TestMethod]
        public void LargeColumn()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new TicTacToeMove(0, 3));
        }

        /// <summary>
        /// Creates a tic-tac-toe move that is in the bounds of the grid
        /// </summary>
        [TestMethod]
        public void InBoundsMove()
        {
            var move = new TicTacToeMove(2, 1);

            Assert.AreEqual(2u, move.Row);
            Assert.AreEqual(1u, move.Column);
        }
    }
}
