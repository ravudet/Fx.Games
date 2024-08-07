namespace Fx.Games.Displayer
{
    using System;
    using System.Linq;

    using Fx.Games.Game;

    /// <summary>
    /// Writes the state of the board and moves of <see cref="PegGame{TPlayer}"/>s to the console
    /// </summary>
    /// <typeparam name="TPlayer">The type of the player that is playing the game</typeparam>
    public sealed class PegGameConsoleDisplayer<TPlayer> : IDisplayer<PegGame<TPlayer>, PegBoard, PegMove, TPlayer>
    {
        /// <summary>
        /// Prevents the initialization of the <see cref="PegGameConsoleDisplayer{TPlayer}"/> class
        /// </summary>
        private PegGameConsoleDisplayer()
        {
        }

        /// <summary>
        /// The singleton instance of <see cref="PegGameConsoleDisplayer{TPlayer}"/>
        /// </summary>
        public static PegGameConsoleDisplayer<TPlayer> Instance { get; } = new PegGameConsoleDisplayer<TPlayer>();

        /// <inheritdoc/>
        public void DisplayAvailableMoves(PegGame<TPlayer> game)
        {
            Console.WriteLine("Available moves:");
            int i = 0;
            foreach (var move in game.Moves)
            {
                Console.WriteLine($"{i++}: {TranscribeMove(move)}");
            }

            Console.WriteLine();
        }

        /// <inheritdoc/>
        public void DisplayBoard(PegGame<TPlayer> game)
        {
            for (int i = 0; i < game.Board.Triangle.Length; ++i)
            {
                Console.Write(new string(' ', game.Board.Triangle.Length - i));
                for (int j = 0; j < game.Board.Triangle[i].Length; ++j)
                {
                    Console.Write(game.Board.Triangle[i][j] == Peg.Empty ? "o " : "* ");
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }

        /// <inheritdoc/>
        public void DisplayOutcome(PegGame<TPlayer> game)
        {
            if (game.WinnersAndLosers.Winners.Any())
            {
                Console.WriteLine("You win!");
            }
            else
            {
                Console.WriteLine("You lose!");
            }
        }

        /// <inheritdoc/>
        public void DisplaySelectedMove(PegMove move)
        {
            Console.WriteLine($"The selected move was {TranscribeMove(move)}");
            Console.WriteLine();
        }

        /// <summary>
        /// Translates a <see cref="PegMove"/> into a human-readable string representing that move
        /// </summary>
        /// <param name="move">The move to translate</param>
        /// <returns>A human-readable string representing <paramref name="move"/></returns>
        private static string TranscribeMove(PegMove move)
        {
            return $"({move.Start.Row},{move.Start.Column}) => ({move.End.Row}, {move.End.Column})";
        }
    }
}
