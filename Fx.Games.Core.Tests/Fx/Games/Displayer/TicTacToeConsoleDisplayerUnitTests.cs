namespace Fx.Games.Displayer
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Fx.Games.Game;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for <see cref="TicTacToeConsoleDisplayer{TPlayer}"/>
    /// </summary>
    [TestClass]
    public sealed class TicTacToeConsoleDisplayerUnitTests
    {
        /// <summary>
        /// Creates a <see cref="TicTacToeConsoleDisplayer{TPlayer}"/> with a <see langword="null"/> transcriber
        /// </summary>
        [TestMethod]
        public void NullTranscriber()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new TicTacToeConsoleDisplayer<string>(
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                null
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                ));
        }

        /// <summary>
        /// Diplays a single move
        /// </summary>
        [TestMethod]
        public void DisplayMove()
        {
            var displayer = new TicTacToeConsoleDisplayer<string>(_ => _);
            var stringBuilder = new StringBuilder();
            using (var textWriter = new StringWriter(stringBuilder))
            {
                lock (ConsoleUtilities.ConsoleLock)
                {
                    var stdOut = Console.Out;
                    try
                    {
                        Console.SetOut(textWriter);
                        displayer.DisplaySelectedMove(new TicTacToeMove(0, 2));
                    }
                    finally
                    {
                        Console.SetOut(stdOut);
                    }
                }
            }
        }

        /// <summary>
        /// Diplays the available moves
        /// </summary>
        [TestMethod]
        public void DisplayAvailableMoves()
        {
            var displayer = new TicTacToeConsoleDisplayer<string>(_ => _);
            var ticTacToe = new TicTacToe<string>("exes", "ohs");

            var stringBuilder = new StringBuilder();
            using (var textWriter = new StringWriter(stringBuilder))
            {
                lock (ConsoleUtilities.ConsoleLock)
                {
                    var stdOut = Console.Out;
                    try
                    {
                        Console.SetOut(textWriter);
                        displayer.DisplayAvailableMoves(ticTacToe);
                    }
                    finally
                    {
                        Console.SetOut(stdOut);
                    }
                }
            }
        }

        /// <summary>
        /// Displays a <see langword="null"/> board
        /// </summary>
        [TestMethod]
        public void DisplayNullBoard()
        {
            var displayer = new TicTacToeConsoleDisplayer<string>(_ => _);
            Assert.ThrowsException<ArgumentNullException>(() => displayer.DisplayBoard(
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                null
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                ));
        }

        /// <summary>
        /// Displays the board
        /// </summary>
        [TestMethod]
        public void DisplayBoard()
        {
            var displayer = new TicTacToeConsoleDisplayer<string>(_ => _);
            var ticTacToe = new TicTacToe<string>("exes", "ohs");
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(0, 0));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(1, 1));

            var stringBuilder = new StringBuilder();
            using (var textWriter = new StringWriter(stringBuilder))
            {
                lock (ConsoleUtilities.ConsoleLock)
                {
                    var stdOut = Console.Out;
                    try
                    {
                        Console.SetOut(textWriter);
                        displayer.DisplayBoard(ticTacToe);
                    }
                    finally
                    {
                        Console.SetOut(stdOut);
                    }
                }
            }
        }

        /// <summary>
        /// Displays a <see langword="null"/> outcome
        /// </summary>
        [TestMethod]
        public void DisplayNullOutcome()
        {
            var displayer = new TicTacToeConsoleDisplayer<string>(_ => _);
            Assert.ThrowsException<ArgumentNullException>(() => displayer.DisplayOutcome(
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                null
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
                ));
        }

        /// <summary>
        /// Diplays a loser
        /// </summary>
        [TestMethod]
        public void DisplayLoser()
        {
            var displayer = new TicTacToeConsoleDisplayer<string>(_ => _);
            var ticTacToe = new TicTacToe<string>("exes", "ohs");
            while (!ticTacToe.IsGameOver)
            {
                ticTacToe = ticTacToe.CommitMove(ticTacToe.Moves.First());
            }

            var stringBuilder = new StringBuilder();
            using (var textWriter = new StringWriter(stringBuilder))
            {
                lock (ConsoleUtilities.ConsoleLock)
                {
                    var stdOut = Console.Out;
                    try
                    {
                        Console.SetOut(textWriter);
                        displayer.DisplayOutcome(ticTacToe);
                    }
                    finally
                    {
                        Console.SetOut(stdOut);
                    }
                }
            }
        }

        /// <summary>
        /// Displays a winner
        /// </summary>
        [TestMethod]
        public void DisplayWinner()
        {
            var displayer = new TicTacToeConsoleDisplayer<string>(_ => _);
            var ticTacToe = new TicTacToe<string>("exes", "ohs");
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(0, 0));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(1, 0));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(0, 1));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(2, 0));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(0, 2));

            var stringBuilder = new StringBuilder();
            using (var textWriter = new StringWriter(stringBuilder))
            {
                lock (ConsoleUtilities.ConsoleLock)
                {
                    var stdOut = Console.Out;
                    try
                    {
                        Console.SetOut(textWriter);
                        displayer.DisplayOutcome(ticTacToe);
                    }
                    finally
                    {
                        Console.SetOut(stdOut);
                    }
                }
            }
        }

        /// <summary>
        /// Displays a draw
        /// </summary>
        [TestMethod]
        public void DisplayDraw()
        {
            var displayer = new TicTacToeConsoleDisplayer<string>(_ => _);
            var ticTacToe = new TicTacToe<string>("exes", "ohs");
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(0, 0));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(1, 1));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(1, 0));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(2, 0));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(0, 2));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(0, 1));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(2, 1));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(2, 2));
            ticTacToe = ticTacToe.CommitMove(new TicTacToeMove(1, 2));

            var stringBuilder = new StringBuilder();
            using (var textWriter = new StringWriter(stringBuilder))
            {
                lock (ConsoleUtilities.ConsoleLock)
                {
                    var stdOut = Console.Out;
                    try
                    {
                        Console.SetOut(textWriter);
                        displayer.DisplayOutcome(ticTacToe);
                    }
                    finally
                    {
                        Console.SetOut(stdOut);
                    }
                }
            }
        }
    }
}
