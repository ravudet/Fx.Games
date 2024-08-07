namespace Fx.Games.Game
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for <see cref="PegBoard"/>
    /// </summary>
    [TestClass]
    public sealed class PegBoardUnitTests
    {
        /// <summary>
        /// Creates a new peg board from a <see langword="null"/> collection of empties
        /// </summary>
        [TestMethod]
        public void NullEmpties()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new PegBoard(
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                // SUPPRESSION test case for the null validation
                null
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                ));
        }

        /// <summary>
        /// Creates a new peg board
        /// </summary>
        [TestMethod]
        public void NewBoard()
        {
            var pegBoard = new PegBoard(new[] { new PegPosition(0, 0) });
            var triangle = pegBoard.Triangle;
            for (int i = 0; i < triangle.Length; ++i)
            {
                var row = triangle[i];
                for (int j = 0; j < row.Length; ++j)
                {
                    if (i == 0 && j == 0)
                    {
                        Assert.AreEqual(Peg.Empty, row[j]);
                    }
                    else
                    {
                        Assert.AreEqual(Peg.Peg, row[j]);
                    }
                }
            }
        }

        /// <summary>
        /// Ensures that the triangle is cloned before it is returned
        /// </summary>
        [TestMethod]
        public void ClonedTriangle()
        {
            var pegBoard = new PegBoard(new[] { new PegPosition(0, 0) });

            Assert.AreEqual(Peg.Empty, pegBoard.Triangle[0][0]);

            pegBoard.Triangle[0][0] = Peg.Peg;

            Assert.AreEqual(Peg.Empty, pegBoard.Triangle[0][0]);
        }
    }
}
