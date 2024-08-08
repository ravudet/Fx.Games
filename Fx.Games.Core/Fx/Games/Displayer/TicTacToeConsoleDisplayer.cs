namespace Fx.Games.Displayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Fx.Games.Game;

    /// <summary>
    /// Writes the state of the board and moves of <see cref="TicTacToe{TPlayer}"/> games to the console
    /// </summary>
    /// <typeparam name="TPlayer">The type of the player that is playing the game</typeparam>
    public sealed class TicTacToeConsoleDisplayer<TPlayer> : IDisplayer<TicTacToe<TPlayer>, TicTacToeBoard, TicTacToeMove, TPlayer>
    {
        private readonly Func<TPlayer, string> playerToString;

        public TicTacToeConsoleDisplayer(Func<TPlayer, string> playerToString)
        {
            if (playerToString == null)
            {
                throw new ArgumentNullException(nameof(playerToString));
            }

            this.playerToString = playerToString;
        }

        public void DisplayBoard(TicTacToe<TPlayer> game)
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game));
            }

            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 2; ++j)
                {
                    Console.Write($"{FromPiece(game.Board.Grid[i, j])}|");
                }

                Console.Write($"{FromPiece(game.Board.Grid[i, 2])}");

                Console.WriteLine();
            }
        }

        public void DisplayOutcome(TicTacToe<TPlayer> game)
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game));
            }

            try
            {
                var winner = game.WinnersAndLosers.Winners.First();
                Console.WriteLine($"{playerToString(winner)} wins!");
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("The game was a draw...");
            }
        }

        public void DisplayAvailableMoves(TicTacToe<TPlayer> game)
        {
            Console.WriteLine("Select a move (row, column):");
            int i = 0;
            foreach (var move in game.Moves)
            {
                Console.WriteLine($"{i++}: ({move.Row}, {move.Column})");
            }

            Console.WriteLine();
        }

        public void DisplaySelectedMove(TicTacToeMove move)
        {
            Console.WriteLine($"({move.Row}, {move.Column})");
        }

        private static char FromPiece(TicTacToePiece piece)
        {
            switch (piece)
            {
                case TicTacToePiece.Empty:
                    return '*';
                case TicTacToePiece.Ex:
                    return 'X';
                case TicTacToePiece.Oh:
                    return 'O';
            }

            throw new InvalidOperationException("TODO");
        }
    }
}
