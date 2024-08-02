namespace Fx.Games.Strategy
{
    using System.Linq;
    using System;

    using Fx.Games.Game;

    /// <summary>
    /// A <see cref="IStrategy{TGame, TBoard, TMove, TPlayer}"/> that requests user input from the console to select a move for each game state
    /// </summary>
    /// <typeparam name="TGame">The type of the game that the strategy can play</typeparam>
    /// <typeparam name="TBoard">The type of the board that the <typeparamref name="TGame"/> uses</typeparam>
    /// <typeparam name="TMove">The type of the moves that the <typeparamref name="TGame"/> uses</typeparam>
    /// <typeparam name="TPlayer">The type of the player that is playing the <typeparamref name="TGame"/></typeparam>
    public sealed class ConsoleStrategy<TGame, TBoard, TMove, TPlayer> : IStrategy<TGame, TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        /// <summary>
        /// Prevents the initialization of the <see cref="ConsoleStrategy{TGame, TBoard, TMove, TPlayer}"/> class
        /// </summary>
        private ConsoleStrategy()
        {
        }

        /// <summary>
        /// Gets the singleton instance of <see cref="ConsoleStrategy{TGame, TBoard, TMove, TPlayer}"/>
        /// </summary>
        public static ConsoleStrategy<TGame, TBoard, TMove, TPlayer> Instance { get; } = new ConsoleStrategy<TGame, TBoard, TMove, TPlayer>();

        /// <inheritdoc/>
        public TMove SelectMove(TGame game)
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game));
            }

            Console.WriteLine("Enter the index of the move you would like to choose:");
            var moves = game.Moves.ToList();
            if (moves.Count == 0)
            {
                throw new InvalidGameException("The game did not have any legal moves to select from");
            }

            while (true)
            {
                var input = Console.ReadLine();
                if (!int.TryParse(input, out var selectedMove) || selectedMove >= moves.Count)
                {
                    Console.WriteLine($"The input '{input}' was not the index of a legal move");
                    continue;
                }

                return moves[selectedMove];
            }
        }
    }
}
