namespace Fx.Games.Displayer
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using Fx.Games.Game;

    /// <summary>
    /// Writes the state of the board and moves of <see cref="TicTacToe{TPlayer}"/> games to the console
    /// </summary>
    /// <typeparam name="TPlayer">The type of the player that is playing the game</typeparam>
    public sealed class TicTacToeConsoleDisplayer<TPlayer> : IDisplayer<TicTacToe<TPlayer>, TicTacToeBoard, TicTacToeMove, TPlayer>
    {
        /// <summary>
        /// Converts the given <see cref="TPlayer"/> into a string representation
        /// </summary>
        private readonly Func<TPlayer, string> playerTranscriber;

        /// <summary>
        /// Initializes a new instance of the <see cref="TicTacToeConsoleDisplayer{TPlayer}"/> class
        /// </summary>
        /// <param name="playerTranscriber">Converts the given <see cref="TPlayer"/> into a string representation</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="playerTranscriber"/> is <see langword="null"/></exception>
        public TicTacToeConsoleDisplayer(Func<TPlayer, string> playerTranscriber)
        {
            if (playerTranscriber == null)
            {
                throw new ArgumentNullException(nameof(playerTranscriber));
            }

            this.playerTranscriber = playerTranscriber;
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public void DisplayOutcome(TicTacToe<TPlayer> game)
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game));
            }

            try
            {
                var winner = game.WinnersAndLosers.Winners.First();
                Console.WriteLine($"{playerTranscriber(winner)} wins!");
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("The game was a draw...");
            }
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public void DisplaySelectedMove(TicTacToeMove move)
        {
            Console.WriteLine($"({move.Row}, {move.Column})");
        }

        /// <summary>
        /// Tranlates <paramref name="piece"/> into a single character representation of the type of that piece
        /// </summary>
        /// <param name="piece">The <see cref="TicTacToePiece"/> to convert</param>
        /// <returns>The single character representation of the type of <paramref name="piece"/></returns>
        /// <exception cref="InvalidOperationException">Thrown if <paramref name="piece"/> is not a known <see cref="TicTacToePiece"/></exception>
        [ExcludeFromCodeCoverage(Justification = "Excluding because there is an unreachable branch")]
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

            throw new InvalidOperationException($"'{nameof(piece)}' had an unknown value: '{(int)piece}'");
        }
    }
}
