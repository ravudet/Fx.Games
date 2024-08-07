namespace Fx.Games.Game
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// Unit test for <see cref="PegGame{TPlayer}"/>
    /// </summary>
    [TestClass]
    public sealed class PegGameUnitTests
    {
        /// <summary>
        /// Plays the peg game always committing the first move
        /// </summary>
        [TestMethod]
        public void FirstMoves()
        {
            var pegGame = new PegGame<string>(nameof(FirstMoves));
            Assert.AreEqual(nameof(FirstMoves), pegGame.CurrentPlayer);
            Assert.IsFalse(pegGame.IsGameOver);
            var emptyPegCount = 0;
            while (!pegGame.IsGameOver)
            {
                Assert.AreEqual(++emptyPegCount, EmptyPegCount(pegGame));
                Assert.IsFalse(pegGame.WinnersAndLosers.Winners.Any());
                Assert.IsFalse(pegGame.WinnersAndLosers.Losers.Any());
                Assert.IsFalse(pegGame.WinnersAndLosers.Drawers.Any());

                var move = pegGame.Moves.First();
                pegGame = pegGame.CommitMove(move);
            }

            Assert.AreEqual(++emptyPegCount, EmptyPegCount(pegGame));
            Assert.IsFalse(pegGame.WinnersAndLosers.Drawers.Any());
        }

        /// <summary>
        /// Plays the peg game always committing the last move
        /// </summary>
        [TestMethod]
        public void LastMoves()
        {
            var pegGame = new PegGame<string>(nameof(FirstMoves));
            Assert.AreEqual(nameof(FirstMoves), pegGame.CurrentPlayer);
            Assert.IsFalse(pegGame.IsGameOver);
            var emptyPegCount = 0;
            while (!pegGame.IsGameOver)
            {
                Assert.AreEqual(++emptyPegCount, EmptyPegCount(pegGame));
                Assert.IsFalse(pegGame.WinnersAndLosers.Winners.Any());
                Assert.IsFalse(pegGame.WinnersAndLosers.Losers.Any());
                Assert.IsFalse(pegGame.WinnersAndLosers.Drawers.Any());

                var move = pegGame.Moves.Last();
                pegGame = pegGame.CommitMove(move);
            }

            Assert.AreEqual(++emptyPegCount, EmptyPegCount(pegGame));
            Assert.IsFalse(pegGame.WinnersAndLosers.Drawers.Any());
        }

        /// <summary>
        /// Plays the peg game always committing the last move
        /// </summary>
        [TestMethod]
        public void WinningSequence()
        {
            var pegGame = new PegGame<string>(nameof(WinningSequence));
            Assert.AreEqual(nameof(WinningSequence), pegGame.CurrentPlayer);

            var emptyPegCount = 0;

            Assert.IsFalse(pegGame.IsGameOver);
            Assert.AreEqual(++emptyPegCount, EmptyPegCount(pegGame));
            Assert.IsFalse(pegGame.WinnersAndLosers.Winners.Any());
            Assert.IsFalse(pegGame.WinnersAndLosers.Losers.Any());
            Assert.IsFalse(pegGame.WinnersAndLosers.Drawers.Any());

            // move 1
            pegGame = pegGame.CommitMove(new PegMove(new PegPosition(2, 0), new PegPosition(0, 0)));
            Assert.IsFalse(pegGame.IsGameOver);
            Assert.AreEqual(++emptyPegCount, EmptyPegCount(pegGame));
            Assert.IsFalse(pegGame.WinnersAndLosers.Winners.Any());
            Assert.IsFalse(pegGame.WinnersAndLosers.Losers.Any());
            Assert.IsFalse(pegGame.WinnersAndLosers.Drawers.Any());

            // move 2
            pegGame = pegGame.CommitMove(new PegMove(new PegPosition(2, 2), new PegPosition(2, 0)));
            Assert.IsFalse(pegGame.IsGameOver);
            Assert.AreEqual(++emptyPegCount, EmptyPegCount(pegGame));
            Assert.IsFalse(pegGame.WinnersAndLosers.Winners.Any());
            Assert.IsFalse(pegGame.WinnersAndLosers.Losers.Any());
            Assert.IsFalse(pegGame.WinnersAndLosers.Drawers.Any());

            // move 3
            pegGame = pegGame.CommitMove(new PegMove(new PegPosition(0, 0), new PegPosition(2, 2)));
            Assert.IsFalse(pegGame.IsGameOver);
            Assert.AreEqual(++emptyPegCount, EmptyPegCount(pegGame));
            Assert.IsFalse(pegGame.WinnersAndLosers.Winners.Any());
            Assert.IsFalse(pegGame.WinnersAndLosers.Losers.Any());
            Assert.IsFalse(pegGame.WinnersAndLosers.Drawers.Any());

            // move 4
            pegGame = pegGame.CommitMove(new PegMove(new PegPosition(3, 0), new PegPosition(1, 0)));
            Assert.IsFalse(pegGame.IsGameOver);
            Assert.AreEqual(++emptyPegCount, EmptyPegCount(pegGame));
            Assert.IsFalse(pegGame.WinnersAndLosers.Winners.Any());
            Assert.IsFalse(pegGame.WinnersAndLosers.Losers.Any());
            Assert.IsFalse(pegGame.WinnersAndLosers.Drawers.Any());

            // move 5
            pegGame = pegGame.CommitMove(new PegMove(new PegPosition(4, 3), new PegPosition(2, 0)));
            Assert.IsFalse(pegGame.IsGameOver);
            Assert.AreEqual(++emptyPegCount, EmptyPegCount(pegGame));
            Assert.IsFalse(pegGame.WinnersAndLosers.Winners.Any());
            Assert.IsFalse(pegGame.WinnersAndLosers.Losers.Any());
            Assert.IsFalse(pegGame.WinnersAndLosers.Drawers.Any());

            // move 6
            pegGame = pegGame.CommitMove(new PegMove(new PegPosition(1, 0), new PegPosition(3, 0)));
            Assert.IsFalse(pegGame.IsGameOver);
            Assert.AreEqual(++emptyPegCount, EmptyPegCount(pegGame));
            Assert.IsFalse(pegGame.WinnersAndLosers.Winners.Any());
            Assert.IsFalse(pegGame.WinnersAndLosers.Losers.Any());
            Assert.IsFalse(pegGame.WinnersAndLosers.Drawers.Any());

            // move 7
            pegGame = pegGame.CommitMove(new PegMove(new PegPosition(3, 3), new PegPosition(3, 1)));
            Assert.IsFalse(pegGame.IsGameOver);
            Assert.AreEqual(++emptyPegCount, EmptyPegCount(pegGame));
            Assert.IsFalse(pegGame.WinnersAndLosers.Winners.Any());
            Assert.IsFalse(pegGame.WinnersAndLosers.Losers.Any());
            Assert.IsFalse(pegGame.WinnersAndLosers.Drawers.Any());

            // move 8
            pegGame = pegGame.CommitMove(new PegMove(new PegPosition(3, 0), new PegPosition(3, 2)));
            Assert.IsFalse(pegGame.IsGameOver);
            Assert.AreEqual(++emptyPegCount, EmptyPegCount(pegGame));
            Assert.IsFalse(pegGame.WinnersAndLosers.Winners.Any());
            Assert.IsFalse(pegGame.WinnersAndLosers.Losers.Any());
            Assert.IsFalse(pegGame.WinnersAndLosers.Drawers.Any());

            // move 9
            pegGame = pegGame.CommitMove(new PegMove(new PegPosition(4, 4), new PegPosition(4, 2)));
            Assert.IsFalse(pegGame.IsGameOver);
            Assert.AreEqual(++emptyPegCount, EmptyPegCount(pegGame));
            Assert.IsFalse(pegGame.WinnersAndLosers.Winners.Any());
            Assert.IsFalse(pegGame.WinnersAndLosers.Losers.Any());
            Assert.IsFalse(pegGame.WinnersAndLosers.Drawers.Any());

            // move 10
            pegGame = pegGame.CommitMove(new PegMove(new PegPosition(4, 1), new PegPosition(4, 3)));
            Assert.IsFalse(pegGame.IsGameOver);
            Assert.AreEqual(++emptyPegCount, EmptyPegCount(pegGame));
            Assert.IsFalse(pegGame.WinnersAndLosers.Winners.Any());
            Assert.IsFalse(pegGame.WinnersAndLosers.Losers.Any());
            Assert.IsFalse(pegGame.WinnersAndLosers.Drawers.Any());

            // move 11
            pegGame = pegGame.CommitMove(new PegMove(new PegPosition(2, 2), new PegPosition(4, 2)));
            Assert.IsFalse(pegGame.IsGameOver);
            Assert.AreEqual(++emptyPegCount, EmptyPegCount(pegGame));
            Assert.IsFalse(pegGame.WinnersAndLosers.Winners.Any());
            Assert.IsFalse(pegGame.WinnersAndLosers.Losers.Any());
            Assert.IsFalse(pegGame.WinnersAndLosers.Drawers.Any());

            // move 12
            pegGame = pegGame.CommitMove(new PegMove(new PegPosition(4, 3), new PegPosition(4, 1)));
            Assert.IsFalse(pegGame.IsGameOver);
            Assert.AreEqual(++emptyPegCount, EmptyPegCount(pegGame));
            Assert.IsFalse(pegGame.WinnersAndLosers.Winners.Any());
            Assert.IsFalse(pegGame.WinnersAndLosers.Losers.Any());
            Assert.IsFalse(pegGame.WinnersAndLosers.Drawers.Any());

            // move 13
            pegGame = pegGame.CommitMove(new PegMove(new PegPosition(4, 0), new PegPosition(4, 2)));

            Assert.IsTrue(pegGame.IsGameOver);
            Assert.AreEqual(++emptyPegCount, EmptyPegCount(pegGame));
            CollectionAssert.AreEqual(new[] { nameof(WinningSequence) }, pegGame.WinnersAndLosers.Winners.ToArray());
            Assert.IsFalse(pegGame.WinnersAndLosers.Losers.Any());
            Assert.IsFalse(pegGame.WinnersAndLosers.Drawers.Any());
        }

        /// <summary>
        /// Plays the peg game committing moves that were randomly generated by <see cref="RandomMoves"/>
        /// </summary>
        [TestMethod]
        public void RandomTest1()
        {
            var player = "player";
            var pegGame = new PegGame<string>(player);

            // begin replacing with output from RandomMoves
            var moveSequences = new[]
            {
                (
                    new PegMove(new PegPosition(2, 2), new PegPosition(0, 0)),
                    new PegBoard(new[]
                    {
                        new PegPosition(1, 1), 
                        new PegPosition(2, 2),
                    })
                ),
                (
                    new PegMove(new PegPosition(4, 4), new PegPosition(2, 2)), 
                    new PegBoard(new[] 
                    { 
                        new PegPosition(1, 1), 
                        new PegPosition(3, 3), 
                        new PegPosition(4, 4), 
                    })
                ),
                (
                    new PegMove(new PegPosition(3, 1), new PegPosition(3, 3)), 
                    new PegBoard(new[] 
                    { 
                        new PegPosition(1, 1), 
                        new PegPosition(3, 1),
                        new PegPosition(3, 2),
                        new PegPosition(4, 4),
                    })
                ),
                (
                    new PegMove(new PegPosition(4, 2), new PegPosition(4, 4)), 
                    new PegBoard(new[] 
                    {
                        new PegPosition(1, 1),
                        new PegPosition(3, 1),
                        new PegPosition(3, 2),
                        new PegPosition(4, 2),
                        new PegPosition(4, 3), 
                    })
                ),
                (
                    new PegMove(new PegPosition(3, 3), new PegPosition(1, 1)), 
                    new PegBoard(new[] 
                    {
                        new PegPosition(2, 2),
                        new PegPosition(3, 1),
                        new PegPosition(3, 2),
                        new PegPosition(3, 3),
                        new PegPosition(4, 2),
                        new PegPosition(4, 3), 
                    })
                ),
                (
                    new PegMove(new PegPosition(4, 0), new PegPosition(4, 2)), 
                    new PegBoard(new[] 
                    {
                        new PegPosition(2, 2),
                        new PegPosition(3, 1),
                        new PegPosition(3, 2),
                        new PegPosition(3, 3),
                        new PegPosition(4, 0),
                        new PegPosition(4, 1),
                        new PegPosition(4, 3),
                    })
                ),
                (
                    new PegMove(new PegPosition(1, 0), new PegPosition(3, 2)), 
                    new PegBoard(new[] 
                    {
                        new PegPosition(1, 0),
                        new PegPosition(2, 1),
                        new PegPosition(2, 2),
                        new PegPosition(3, 1),
                        new PegPosition(3, 3),
                        new PegPosition(4, 0),
                        new PegPosition(4, 1),
                        new PegPosition(4, 3), 
                    })
                ),
                (
                    new PegMove(new PegPosition(0, 0), new PegPosition(2, 2)), 
                    new PegBoard(new[] 
                    {
                        new PegPosition(0, 0),
                        new PegPosition(1, 0),
                        new PegPosition(1, 1),
                        new PegPosition(2, 1),
                        new PegPosition(3, 1),
                        new PegPosition(3, 3),
                        new PegPosition(4, 0),
                        new PegPosition(4, 1),
                        new PegPosition(4, 3), 
                    })
                ),
                (
                    new PegMove(new PegPosition(2, 0), new PegPosition(4, 0)), 
                    new PegBoard(new[] 
                    {
                        new PegPosition(0, 0),
                        new PegPosition(1, 0),
                        new PegPosition(1, 1),
                        new PegPosition(2, 0),
                        new PegPosition(2, 1),
                        new PegPosition(3, 0),
                        new PegPosition(3, 1),
                        new PegPosition(3, 3),
                        new PegPosition(4, 1),
                        new PegPosition(4, 3),
                    })
                ),
            };
            var winner = false;
            // end replacing with output from RandomMoves

            Assert.AreEqual("player", pegGame.CurrentPlayer);
            Assert.IsFalse(pegGame.IsGameOver);
            for (int i = 0; i < moveSequences.Length; ++i)
            {
                Trace.WriteLine($"Beginning move sequence '{i}'");
                Assert.IsFalse(pegGame.WinnersAndLosers.Winners.Any());
                Assert.IsFalse(pegGame.WinnersAndLosers.Losers.Any());
                Assert.IsFalse(pegGame.WinnersAndLosers.Drawers.Any());

                var moveSequence = moveSequences[i];
                pegGame = pegGame.CommitMove(moveSequence.Item1);
                AssertBoardEquality(moveSequence.Item2, pegGame.Board);
            }

            AssertBoardEquality(moveSequences[moveSequences.Length - 1].Item2, pegGame.Board);
            Assert.IsTrue(pegGame.IsGameOver);
            Assert.AreEqual(winner, pegGame.WinnersAndLosers.Winners.Contains(player));
            Assert.AreNotEqual(winner, pegGame.WinnersAndLosers.Losers.Contains(player));
        }

        /// <summary>
        /// Plays the peg game committing moves that were randomly generated by <see cref="RandomMoves"/>
        /// </summary>
        [TestMethod]
        public void RandomTest2()
        {
            var player = "player";
            var pegGame = new PegGame<string>(player);

            // begin replacing with output from RandomMoves
            var moveSequences = new[]
            {
                (
                    new PegMove(new PegPosition(2, 2), new PegPosition(0, 0)), 
                    new PegBoard(new[] 
                    {
                        new PegPosition(1, 1),
                        new PegPosition(2, 2), 
                    })
                ),
                (
                    new PegMove(new PegPosition(4, 4), new PegPosition(2, 2)), 
                    new PegBoard(new[] 
                    {
                        new PegPosition(1, 1),
                        new PegPosition(3, 3),
                        new PegPosition(4, 4), 
                    })
                ),
                (
                    new PegMove(new PegPosition(4, 2), new PegPosition(4, 4)), 
                    new PegBoard(new[] 
                    {
                        new PegPosition(1, 1),
                        new PegPosition(3, 3),
                        new PegPosition(4, 2),
                        new PegPosition(4, 3), 
                    })
                ),
                (
                    new PegMove(new PegPosition(2, 0), new PegPosition(4, 2)), 
                    new PegBoard(new[] 
                    {
                        new PegPosition(1, 1),
                        new PegPosition(2, 0),
                        new PegPosition(3, 1),
                        new PegPosition(3, 3),
                        new PegPosition(4, 3), 
                    })
                ),
                (
                    new PegMove(new PegPosition(2, 1), new PegPosition(4, 3)), 
                    new PegBoard(new[] 
                    {
                        new PegPosition(1, 1),
                        new PegPosition(2, 0),
                        new PegPosition(2, 1),
                        new PegPosition(3, 1),
                        new PegPosition(3, 2),
                        new PegPosition(3, 3), 
                    })
                ),
                (
                    new PegMove(new PegPosition(0, 0), new PegPosition(2, 0)), 
                    new PegBoard(new[]
                    {
                        new PegPosition(0, 0),
                        new PegPosition(1, 0),
                        new PegPosition(1, 1),
                        new PegPosition(2, 1),
                        new PegPosition(3, 1),
                        new PegPosition(3, 2),
                        new PegPosition(3, 3), 
                    })
                ),
                (
                    new PegMove(new PegPosition(3, 0), new PegPosition(1, 0)), 
                    new PegBoard(new[] 
                    {
                        new PegPosition(0, 0),
                        new PegPosition(1, 1),
                        new PegPosition(2, 0),
                        new PegPosition(2, 1),
                        new PegPosition(3, 0),
                        new PegPosition(3, 1),
                        new PegPosition(3, 2),
                        new PegPosition(3, 3), 
                    })
                ),
            };
            var winner = false;
            // end replacing with output from RandomMoves

            Assert.AreEqual("player", pegGame.CurrentPlayer);
            Assert.IsFalse(pegGame.IsGameOver);
            for (int i = 0; i < moveSequences.Length; ++i)
            {
                Assert.IsFalse(pegGame.WinnersAndLosers.Winners.Any());
                Assert.IsFalse(pegGame.WinnersAndLosers.Losers.Any());
                Assert.IsFalse(pegGame.WinnersAndLosers.Drawers.Any());

                var moveSequence = moveSequences[i];
                pegGame = pegGame.CommitMove(moveSequence.Item1);
                AssertBoardEquality(moveSequence.Item2, pegGame.Board);
            }

            AssertBoardEquality(moveSequences[moveSequences.Length - 1].Item2, pegGame.Board);
            Assert.IsTrue(pegGame.IsGameOver);
            Assert.AreEqual(winner, pegGame.WinnersAndLosers.Winners.Contains(player));
            Assert.AreNotEqual(winner, pegGame.WinnersAndLosers.Losers.Contains(player));
        }

        /// <summary>
        /// Plays the peg game committing moves that were randomly generated by <see cref="RandomMoves"/>
        /// </summary>
        [TestMethod]
        public void RandomTest3()
        {
            var player = "player";
            var pegGame = new PegGame<string>(player);

            // begin replacing with output from RandomMoves
            var moveSequences = new[]
            {
                (
                    new PegMove(new PegPosition(2, 2), new PegPosition(0, 0)), 
                    new PegBoard(new[]
                    {
                        new PegPosition(1, 1),
                        new PegPosition(2, 2),
                    })
                ),
                (
                    new PegMove(new PegPosition(3, 1), new PegPosition(1, 1)), 
                    new PegBoard(new[]
                    {
                        new PegPosition(2, 1),
                        new PegPosition(2, 2),
                        new PegPosition(3, 1),
                    })
                ),
                (
                    new PegMove(new PegPosition(0, 0), new PegPosition(2, 2)), 
                    new PegBoard(new[] 
                    {
                        new PegPosition(0, 0),
                        new PegPosition(1, 1),
                        new PegPosition(2, 1),
                        new PegPosition(3, 1),
                    })
                ),
                (
                    new PegMove(new PegPosition(4, 3), new PegPosition(2, 1)), 
                    new PegBoard(new[] 
                    {
                        new PegPosition(0, 0),
                        new PegPosition(1, 1),
                        new PegPosition(3, 1),
                        new PegPosition(3, 2),
                        new PegPosition(4, 3),
                    })
                ),
                (
                    new PegMove(new PegPosition(3, 3), new PegPosition(1, 1)),
                    new PegBoard(new[] 
                    {
                        new PegPosition(0, 0),
                        new PegPosition(2, 2),
                        new PegPosition(3, 1),
                        new PegPosition(3, 2),
                        new PegPosition(3, 3),
                        new PegPosition(4, 3), 
                    })
                ),
                (
                    new PegMove(new PegPosition(2, 0), new PegPosition(2, 2)),
                    new PegBoard(new[]
                    {
                        new PegPosition(0, 0),
                        new PegPosition(2, 0),
                        new PegPosition(2, 1),
                        new PegPosition(3, 1),
                        new PegPosition(3, 2),
                        new PegPosition(3, 3),
                        new PegPosition(4, 3),
                    })
                ),
                (
                    new PegMove(new PegPosition(2, 2), new PegPosition(0, 0)), 
                    new PegBoard(new[] 
                    {
                        new PegPosition(1, 1),
                        new PegPosition(2, 0),
                        new PegPosition(2, 1),
                        new PegPosition(2, 2),
                        new PegPosition(3, 1),
                        new PegPosition(3, 2),
                        new PegPosition(3, 3),
                        new PegPosition(4, 3), 
                    })
                ),
                (
                    new PegMove(new PegPosition(4, 1), new PegPosition(4, 3)), 
                    new PegBoard(new[]
                    {
                        new PegPosition(1, 1),
                        new PegPosition(2, 0),
                        new PegPosition(2, 1),
                        new PegPosition(2, 2),
                        new PegPosition(3, 1),
                        new PegPosition(3, 2),
                        new PegPosition(3, 3),
                        new PegPosition(4, 1),
                        new PegPosition(4, 2), 
                    })
                ),
                (
                    new PegMove(new PegPosition(4, 4), new PegPosition(4, 2)), 
                    new PegBoard(new[]
                    {
                        new PegPosition(1, 1),
                        new PegPosition(2, 0),
                        new PegPosition(2, 1),
                        new PegPosition(2, 2),
                        new PegPosition(3, 1),
                        new PegPosition(3, 2),
                        new PegPosition(3, 3),
                        new PegPosition(4, 1),
                        new PegPosition(4, 3),
                        new PegPosition(4, 4),
                    })
                ),
                (
                    new PegMove(new PegPosition(4, 0), new PegPosition(2, 0)), 
                    new PegBoard(new[] 
                    {
                        new PegPosition(1, 1),
                        new PegPosition(2, 1),
                        new PegPosition(2, 2),
                        new PegPosition(3, 0),
                        new PegPosition(3, 1),
                        new PegPosition(3, 2),
                        new PegPosition(3, 3),
                        new PegPosition(4, 0),
                        new PegPosition(4, 1),
                        new PegPosition(4, 3),
                        new PegPosition(4, 4),
                    })
                ),
                (
                    new PegMove(new PegPosition(1, 0), new PegPosition(3, 0)), 
                    new PegBoard(new[] 
                    {
                        new PegPosition(1, 0),
                        new PegPosition(1, 1),
                        new PegPosition(2, 0),
                        new PegPosition(2, 1),
                        new PegPosition(2, 2),
                        new PegPosition(3, 1),
                        new PegPosition(3, 2),
                        new PegPosition(3, 3),
                        new PegPosition(4, 0),
                        new PegPosition(4, 1),
                        new PegPosition(4, 3),
                        new PegPosition(4, 4),
                    })
                ),
            };
            var winner = false;
            // end replacing with output from RandomMoves

            Assert.AreEqual("player", pegGame.CurrentPlayer);
            Assert.IsFalse(pegGame.IsGameOver);
            for (int i = 0; i < moveSequences.Length; ++i)
            {
                Assert.IsFalse(pegGame.WinnersAndLosers.Winners.Any());
                Assert.IsFalse(pegGame.WinnersAndLosers.Losers.Any());
                Assert.IsFalse(pegGame.WinnersAndLosers.Drawers.Any());

                var moveSequence = moveSequences[i];
                pegGame = pegGame.CommitMove(moveSequence.Item1);
                AssertBoardEquality(moveSequence.Item2, pegGame.Board);
            }

            AssertBoardEquality(moveSequences[moveSequences.Length - 1].Item2, pegGame.Board);
            Assert.IsTrue(pegGame.IsGameOver);
            Assert.AreEqual(winner, pegGame.WinnersAndLosers.Winners.Contains(player));
            Assert.AreNotEqual(winner, pegGame.WinnersAndLosers.Losers.Contains(player));
        }

        /// <summary>
        /// Plays the peg game committing moves that were randomly generated by <see cref="RandomMoves"/>
        /// </summary>
        [TestMethod]
        public void RandomTest4()
        {
            var player = "player";
            var pegGame = new PegGame<string>(player);

            // begin replacing with output from RandomMoves
            var moveSequences = new[]
            {
                (
                    new PegMove(new PegPosition(2, 0), new PegPosition(0, 0)), 
                    new PegBoard(new[] 
                    {
                        new PegPosition(1, 0),
                        new PegPosition(2, 0), 
                    })
                ),
                (
                    new PegMove(new PegPosition(4, 2), new PegPosition(2, 0)), 
                    new PegBoard(new[] 
                    {
                        new PegPosition(1, 0),
                        new PegPosition(3, 1),
                        new PegPosition(4, 2), 
                    })
                ),
                (
                    new PegMove(new PegPosition(4, 4), new PegPosition(4, 2)),
                    new PegBoard(new[] 
                    {
                        new PegPosition(1, 0),
                        new PegPosition(3, 1),
                        new PegPosition(4, 3),
                        new PegPosition(4, 4),
                    })
                ),
                (
                    new PegMove(new PegPosition(1, 1), new PegPosition(3, 1)),
                    new PegBoard(new[] 
                    {
                        new PegPosition(1, 0),
                        new PegPosition(1, 1),
                        new PegPosition(2, 1),
                        new PegPosition(4, 3),
                        new PegPosition(4, 4), 
                    })
                ),
                (
                    new PegMove(new PegPosition(4, 1), new PegPosition(2, 1)),
                    new PegBoard(new[] 
                    {
                        new PegPosition(1, 0),
                        new PegPosition(1, 1),
                        new PegPosition(3, 1),
                        new PegPosition(4, 1),
                        new PegPosition(4, 3),
                        new PegPosition(4, 4),
                    })
                ),
                (
                    new PegMove(new PegPosition(3, 3), new PegPosition(1, 1)), 
                    new PegBoard(new[]
                    {
                        new PegPosition(1, 0),
                        new PegPosition(2, 2),
                        new PegPosition(3, 1),
                        new PegPosition(3, 3),
                        new PegPosition(4, 1),
                        new PegPosition(4, 3),
                        new PegPosition(4, 4),
                    })
                ),
                (
                    new PegMove(new PegPosition(3, 0), new PegPosition(1, 0)), 
                    new PegBoard(new[] 
                    {
                        new PegPosition(2, 0),
                        new PegPosition(2, 2),
                        new PegPosition(3, 0),
                        new PegPosition(3, 1),
                        new PegPosition(3, 3),
                        new PegPosition(4, 1),
                        new PegPosition(4, 3),
                        new PegPosition(4, 4),
                    })
                ),
                (
                    new PegMove(new PegPosition(4, 2), new PegPosition(2, 2)),
                    new PegBoard(new[]
                    {
                        new PegPosition(2, 0),
                        new PegPosition(3, 0),
                        new PegPosition(3, 1),
                        new PegPosition(3, 2),
                        new PegPosition(3, 3),
                        new PegPosition(4, 1),
                        new PegPosition(4, 2),
                        new PegPosition(4, 3),
                        new PegPosition(4, 4),
                    })
                ),
                (
                    new PegMove(new PegPosition(1, 0), new PegPosition(3, 2)), 
                    new PegBoard(new[] 
                    {
                        new PegPosition(1, 0),
                        new PegPosition(2, 0),
                        new PegPosition(2, 1),
                        new PegPosition(3, 0),
                        new PegPosition(3, 1),
                        new PegPosition(3, 3),
                        new PegPosition(4, 1),
                        new PegPosition(4, 2),
                        new PegPosition(4, 3),
                        new PegPosition(4, 4),
                    })
                ),
                (
                    new PegMove(new PegPosition(2, 2), new PegPosition(4, 2)), 
                    new PegBoard(new[]
                    {
                        new PegPosition(1, 0),
                        new PegPosition(2, 0),
                        new PegPosition(2, 1),
                        new PegPosition(2, 2),
                        new PegPosition(3, 0),
                        new PegPosition(3, 1),
                        new PegPosition(3, 2),
                        new PegPosition(3, 3),
                        new PegPosition(4, 1),
                        new PegPosition(4, 3),
                        new PegPosition(4, 4),
                    })
                ),
                (
                    new PegMove(new PegPosition(0, 0), new PegPosition(2, 2)), 
                    new PegBoard(new[] 
                    {
                        new PegPosition(0, 0),
                        new PegPosition(1, 0),
                        new PegPosition(1, 1),
                        new PegPosition(2, 0),
                        new PegPosition(2, 1),
                        new PegPosition(3, 0),
                        new PegPosition(3, 1),
                        new PegPosition(3, 2),
                        new PegPosition(3, 3),
                        new PegPosition(4, 1),
                        new PegPosition(4, 3),
                        new PegPosition(4, 4),
                    })
                ),
            };
            var winner = false;
            // end replacing with output from RandomMoves

            Assert.AreEqual("player", pegGame.CurrentPlayer);
            Assert.IsFalse(pegGame.IsGameOver);
            for (int i = 0; i < moveSequences.Length; ++i)
            {
                Assert.IsFalse(pegGame.WinnersAndLosers.Winners.Any());
                Assert.IsFalse(pegGame.WinnersAndLosers.Losers.Any());
                Assert.IsFalse(pegGame.WinnersAndLosers.Drawers.Any());

                var moveSequence = moveSequences[i];
                pegGame = pegGame.CommitMove(moveSequence.Item1);
                AssertBoardEquality(moveSequence.Item2, pegGame.Board);
            }

            AssertBoardEquality(moveSequences[moveSequences.Length - 1].Item2, pegGame.Board);
            Assert.IsTrue(pegGame.IsGameOver);
            Assert.AreEqual(winner, pegGame.WinnersAndLosers.Winners.Contains(player));
            Assert.AreNotEqual(winner, pegGame.WinnersAndLosers.Losers.Contains(player));
        }

        /// <summary>
        /// Plays the peg game committing moves that were randomly generated by <see cref="RandomMoves"/>
        /// </summary>
        [TestMethod]
        public void RandomTest5()
        {
            var player = "player";
            var pegGame = new PegGame<string>(player);

            // begin replacing with output from RandomMoves
            var moveSequences = new[]
            {
                (
                    new PegMove(new PegPosition(2, 2), new PegPosition(0, 0)), 
                    new PegBoard(new[] 
                    {
                        new PegPosition(1, 1),
                        new PegPosition(2, 2),
                    })
                ),
                (
                    new PegMove(new PegPosition(2, 0), new PegPosition(2, 2)), 
                    new PegBoard(new[]
                    {
                        new PegPosition(1, 1),
                        new PegPosition(2, 0),
                        new PegPosition(2, 1),
                    })
                ),
                (
                    new PegMove(new PegPosition(3, 3), new PegPosition(1, 1)), 
                    new PegBoard(new[] 
                    {
                        new PegPosition(2, 0),
                        new PegPosition(2, 1),
                        new PegPosition(2, 2),
                        new PegPosition(3, 3),
                    })
                ),
                (
                    new PegMove(new PegPosition(4, 3), new PegPosition(2, 1)), 
                    new PegBoard(new[] 
                    {
                        new PegPosition(2, 0),
                        new PegPosition(2, 2),
                        new PegPosition(3, 2),
                        new PegPosition(3, 3),
                        new PegPosition(4, 3),
                    })
                ),
                (
                    new PegMove(new PegPosition(4, 2), new PegPosition(2, 0)), 
                    new PegBoard(new[] 
                    {
                        new PegPosition(2, 2),
                        new PegPosition(3, 1),
                        new PegPosition(3, 2),
                        new PegPosition(3, 3),
                        new PegPosition(4, 2),
                        new PegPosition(4, 3),
                    })
                ),
                (
                    new PegMove(new PegPosition(1, 0), new PegPosition(3, 2)), 
                    new PegBoard(new[] 
                    {
                        new PegPosition(1, 0),
                        new PegPosition(2, 1),
                        new PegPosition(2, 2),
                        new PegPosition(3, 1),
                        new PegPosition(3, 3),
                        new PegPosition(4, 2),
                        new PegPosition(4, 3),
                    })
                ),
                (
                    new PegMove(new PegPosition(4, 0), new PegPosition(4, 2)), 
                    new PegBoard(new[] 
                    {
                        new PegPosition(1, 0),
                        new PegPosition(2, 1),
                        new PegPosition(2, 2),
                        new PegPosition(3, 1),
                        new PegPosition(3, 3),
                        new PegPosition(4, 0),
                        new PegPosition(4, 1),
                        new PegPosition(4, 3),
                    })
                ),
                (
                    new PegMove(new PegPosition(4, 2), new PegPosition(2, 2)), 
                    new PegBoard(new[] 
                    {
                        new PegPosition(1, 0),
                        new PegPosition(2, 1),
                        new PegPosition(3, 1),
                        new PegPosition(3, 2),
                        new PegPosition(3, 3),
                        new PegPosition(4, 0),
                        new PegPosition(4, 1),
                        new PegPosition(4, 2),
                        new PegPosition(4, 3), 
                    })
                ),
                (
                    new PegMove(new PegPosition(2, 0), new PegPosition(4, 0)), 
                    new PegBoard(new[] 
                    {
                        new PegPosition(1, 0),
                        new PegPosition(2, 0),
                        new PegPosition(2, 1),
                        new PegPosition(3, 0),
                        new PegPosition(3, 1),
                        new PegPosition(3, 2),
                        new PegPosition(3, 3),
                        new PegPosition(4, 1),
                        new PegPosition(4, 2),
                        new PegPosition(4, 3),
                    })
                ),
                (
                    new PegMove(new PegPosition(1, 1), new PegPosition(3, 3)), 
                    new PegBoard(new[] 
                    {
                        new PegPosition(1, 0),
                        new PegPosition(1, 1),
                        new PegPosition(2, 0),
                        new PegPosition(2, 1),
                        new PegPosition(2, 2),
                        new PegPosition(3, 0),
                        new PegPosition(3, 1),
                        new PegPosition(3, 2),
                        new PegPosition(4, 1),
                        new PegPosition(4, 2),
                        new PegPosition(4, 3),
                    })
                ),
                (
                    new PegMove(new PegPosition(4, 4), new PegPosition(2, 2)), 
                    new PegBoard(new[] 
                    {
                        new PegPosition(1, 0),
                        new PegPosition(1, 1),
                        new PegPosition(2, 0),
                        new PegPosition(2, 1),
                        new PegPosition(3, 0),
                        new PegPosition(3, 1),
                        new PegPosition(3, 2),
                        new PegPosition(3, 3),
                        new PegPosition(4, 1),
                        new PegPosition(4, 2),
                        new PegPosition(4, 3),
                        new PegPosition(4, 4),
                    })
                ),
            };
            var winner = false;
            // end replacing with output from RandomMoves

            Assert.AreEqual("player", pegGame.CurrentPlayer);
            Assert.IsFalse(pegGame.IsGameOver);
            for (int i = 0; i < moveSequences.Length; ++i)
            {
                Assert.IsFalse(pegGame.WinnersAndLosers.Winners.Any());
                Assert.IsFalse(pegGame.WinnersAndLosers.Losers.Any());
                Assert.IsFalse(pegGame.WinnersAndLosers.Drawers.Any());

                var moveSequence = moveSequences[i];
                pegGame = pegGame.CommitMove(moveSequence.Item1);
                AssertBoardEquality(moveSequence.Item2, pegGame.Board);
            }

            AssertBoardEquality(moveSequences[moveSequences.Length - 1].Item2, pegGame.Board);
            Assert.IsTrue(pegGame.IsGameOver);
            Assert.AreEqual(winner, pegGame.WinnersAndLosers.Winners.Contains(player));
            Assert.AreNotEqual(winner, pegGame.WinnersAndLosers.Losers.Contains(player));
        }

        /// <summary>
        /// Plays the peg game committing random moves
        /// </summary>
        [TestMethod]
        public void RandomMoves()
        {
            var pegGame = new PegGame<string>(nameof(RandomMoves));
            var ticks = Environment.TickCount;
            Trace.WriteLine($"Ticks used to seed random: {ticks}");
            var random = new Random(ticks);

            Trace.Indent();
            Trace.Indent();
            Trace.Indent(); //// TODO fix all of the traces
            Trace.WriteLine("var moveSequences = new[]");
            Trace.WriteLine("{");
            Trace.Indent();
            Assert.AreEqual(nameof(RandomMoves), pegGame.CurrentPlayer);
            Assert.IsFalse(pegGame.IsGameOver);
            var emptyPegCount = 0;
            while (!pegGame.IsGameOver)
            {
                Assert.AreEqual(++emptyPegCount, EmptyPegCount(pegGame));
                Assert.IsFalse(pegGame.WinnersAndLosers.Winners.Any());
                Assert.IsFalse(pegGame.WinnersAndLosers.Losers.Any());
                Assert.IsFalse(pegGame.WinnersAndLosers.Drawers.Any());

                var moves = pegGame.Moves.ToList();
                var nextMoveIndex = random.Next(0, moves.Count);
                var nextMove = moves[nextMoveIndex];
                Trace.Write($"(new PegMove(new PegPosition({nextMove.Start.Row}, {nextMove.Start.Column}), new PegPosition({nextMove.End.Row}, {nextMove.End.Column})), new PegBoard(5, new[] {{ ");

                pegGame = pegGame.CommitMove(nextMove);

                TraceEmpties(pegGame.Board);
                Trace.WriteLine("})), ");
            }

            Trace.Unindent();
            Trace.WriteLine("};");

            Assert.AreEqual(++emptyPegCount, EmptyPegCount(pegGame));
            Assert.IsFalse(pegGame.WinnersAndLosers.Drawers.Any());

            Trace.WriteLine($"var winner = {pegGame.WinnersAndLosers.Winners.Any().ToString().ToLowerInvariant()};");
            Trace.Unindent();
            Trace.Unindent();
            Trace.Unindent();
        }

        private static void TraceEmpties(PegBoard pegBoard)
        {
            for (int i = 0; i < pegBoard.Triangle.Length; ++i)
            {
                var row = pegBoard.Triangle[i];
                for (int j = 0; j < row.Length; ++j)
                {
                    if (row[j] == Peg.Empty)
                    {
                        Trace.Write($"({i}, {j}), ");
                    }
                }
            }
        }

        private static void AssertBoardEquality(PegBoard expected, PegBoard actual)
        {
            for (int i = 0; i < expected.Triangle.Length; ++i)
            {
                var row = expected.Triangle[i];
                for (int j = 0; j < row.Length; ++j)
                {
                    Assert.AreEqual(row[j], actual.Triangle[i][j], $"position ({i}, {j})");
                }
            }
        }

        private static int EmptyPegCount<TPlayer>(PegGame<TPlayer> pegGame)
        {
            var count = 0;
            for (int i = 0; i < pegGame.Board.Triangle.Length; ++i)
            {
                var row = pegGame.Board.Triangle[i];
                for (int j = 0; j < row.Length; ++j)
                {
                    if (row[j] == Peg.Empty)
                    {
                        ++count;
                    }
                }
            }

            return count;
        }
    }
}
