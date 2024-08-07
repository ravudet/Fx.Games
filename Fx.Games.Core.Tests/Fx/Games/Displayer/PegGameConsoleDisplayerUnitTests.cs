namespace Fx.Games.Displayer
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Fx.Games.Game;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for <see cref="PegGameConsoleDisplayer{TPlayer}"/>
    /// </summary>
    [TestClass]
    public sealed class PegGameConsoleDisplayerUnitTests
    {
        /// <summary>
        /// Diplays a single move
        /// </summary>
        [TestMethod]
        public void DisplayMove()
        {
            var stringBuilder = new StringBuilder();
            using (var textWriter = new StringWriter(stringBuilder))
            {
                lock (ConsoleUtilities.ConsoleLock)
                {
                    var stdOut = Console.Out;
                    try
                    {
                        Console.SetOut(textWriter);
                        PegGameConsoleDisplayer<string>.Instance.DisplaySelectedMove(new PegMove(new PegPosition(0, 0), new PegPosition(2, 2)));
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
            var pegGame = new PegGame<string>(nameof(DisplayAvailableMoves));

            var stringBuilder = new StringBuilder();
            using (var textWriter = new StringWriter(stringBuilder))
            {
                lock (ConsoleUtilities.ConsoleLock)
                {
                    var stdOut = Console.Out;
                    try
                    {
                        Console.SetOut(textWriter);
                        PegGameConsoleDisplayer<string>.Instance.DisplayAvailableMoves(pegGame);
                    }
                    finally
                    {
                        Console.SetOut(stdOut);
                    }
                }
            }
        }

        /// <summary>
        /// Diplays the board
        /// </summary>
        [TestMethod]
        public void DisplayBoard()
        {
            var pegGame = new PegGame<string>(nameof(DisplayAvailableMoves));

            var stringBuilder = new StringBuilder();
            using (var textWriter = new StringWriter(stringBuilder))
            {
                lock (ConsoleUtilities.ConsoleLock)
                {
                    var stdOut = Console.Out;
                    try
                    {
                        Console.SetOut(textWriter);
                        PegGameConsoleDisplayer<string>.Instance.DisplayBoard(pegGame);
                    }
                    finally
                    {
                        Console.SetOut(stdOut);
                    }
                }
            }
        }

        /// <summary>
        /// Diplays a loser
        /// </summary>
        [TestMethod]
        public void DisplayLoser()
        {
            var pegGame = new PegGame<string>(nameof(DisplayAvailableMoves));
            while (!pegGame.IsGameOver)
            {
                pegGame = pegGame.CommitMove(pegGame.Moves.First());
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
                        PegGameConsoleDisplayer<string>.Instance.DisplayOutcome(pegGame);
                    }
                    finally
                    {
                        Console.SetOut(stdOut);
                    }
                }
            }
        }

        /// <summary>
        /// Diplays a winner
        /// </summary>
        [TestMethod]
        public void DisplayWinner()
        {
            var pegGame = new PegGame<string>(nameof(DisplayAvailableMoves));
            for (int i = 0; i < PegGameUtilities.WinningSequence.Count; ++i)
            {
                pegGame = pegGame.CommitMove(PegGameUtilities.WinningSequence[i]);
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
                        PegGameConsoleDisplayer<string>.Instance.DisplayOutcome(pegGame);
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
