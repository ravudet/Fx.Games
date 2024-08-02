namespace Fx.Games.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Fx.Games.Game;

    /// <summary>
    /// A <see cref="IStrategy{TGame, TBoard, TMove, TPlayer}"/> that selects a random move for each game state
    /// </summary>
    /// <typeparam name="TGame">The type of the game that the strategy can play</typeparam>
    /// <typeparam name="TBoard">The type of the board that the <typeparamref name="TGame"/> uses</typeparam>
    /// <typeparam name="TMove">The type of the moves that the <typeparamref name="TGame"/> uses</typeparam>
    /// <typeparam name="TPlayer">The type of the player that is playing the <typeparamref name="TGame"/></typeparam>
    public sealed class RandomStrategy<TGame, TBoard, TMove, TPlayer> : IStrategy<TGame, TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        /// <summary>
        /// The distribution to use when selecting a random move
        /// </summary>
        private readonly Random random;

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomStrategy{TGame, TBoard, TMove, TPlayer}"/> class
        /// </summary>
        public RandomStrategy()
            : this(RandomStrategySettings<TGame, TBoard, TMove, TPlayer>.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomStrategy{TGame, TBoard, TMove, TPlayer}"/> class
        /// </summary>
        /// <param name="settings">The settings to use to configure the strategy</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="settings"/> is <see langword="null"></exception>
        public RandomStrategy(RandomStrategySettings<TGame, TBoard, TMove, TPlayer> settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            this.random = settings.Random;
        }

        /// <inheritdoc/>
        public TMove SelectMove(TGame game)
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game));
            }

            var moves = game.Moves.ToList();
            if (moves.Count == 0)
            {
                throw new InvalidGameException("The game did not have any legal moves to select from");
            }

            var move = moves.Sample(this.random);
            return move;
        }
    }
}
