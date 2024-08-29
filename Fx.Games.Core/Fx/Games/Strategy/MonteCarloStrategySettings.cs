namespace Fx.Games.Strategy
{
    using System;
    using System.Collections.Generic;

    using Fx.Games.Game;

    /// <summary>
    /// The settings used to instantiate a <see cref="MonteCarloStrategy{TGame, TBoard, TMove, TPlayer}"/>
    /// </summary>
    /// <typeparam name="TGame">The type of the game that the strategy can play</typeparam>
    /// <typeparam name="TBoard">The type of the board that the <typeparamref name="TGame"/> uses</typeparam>
    /// <typeparam name="TMove">The type of the moves that the <typeparamref name="TGame"/> uses</typeparam>
    /// <typeparam name="TPlayer">The type of the player that is playing the <typeparamref name="TGame"/></typeparam>
    public sealed class MonteCarloStrategySettings<TGame, TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MonteCarloStrategySettings{TGame, TBoard, TMove, TPlayer}"/> class
        /// </summary>
        /// <param name="playerComparer">A <see cref="IEqualityComparer{T}"/> that compares players of the game</param>
        /// <param name="random">The distribution to use when selecting random moves when the strategy simulates a game</param>
        private MonteCarloStrategySettings(IEqualityComparer<TPlayer> playerComparer, Random random)
        {
            this.PlayerComparer = playerComparer;
            this.Random = random;
        }

        /// <summary>
        /// The default instance of <see cref="MonteCarloStrategySettings{TGame, TBoard, TMove, TPlayer}"/>
        /// </summary>
        public static MonteCarloStrategySettings<TGame, TBoard, TMove, TPlayer> Default { get; } = new MonteCarloStrategySettings<TGame, TBoard, TMove, TPlayer>(EqualityComparer<TPlayer>.Default, new Random());

        /// <summary>
        /// A <see cref="IEqualityComparer{T}"/> that compares players of the game
        /// </summary>
        public IEqualityComparer<TPlayer> PlayerComparer { get; }

        /// <summary>
        /// The distribution to use when selecting random moves when the strategy simulates a game
        /// </summary>
        public Random Random { get; }

        /// <summary>
        /// A builder for <see cref="MonteCarloStrategySettings{TGame, TBoard, TMove, TPlayer}"/>
        /// </summary>
        public sealed class Builder
        {
            /// <summary>
            /// A <see cref="IEqualityComparer{T}"/> that compares players of the game
            /// </summary>
            public IEqualityComparer<TPlayer> PlayerComparer { get; set; } = MonteCarloStrategySettings<TGame, TBoard, TMove, TPlayer>.Default.PlayerComparer;

            /// <summary>
            /// The distribution to use when selecting random moves when the strategy simulates a game
            /// </summary>
            public Random Random { get; set; } = MonteCarloStrategySettings<TGame, TBoard, TMove, TPlayer>.Default.Random;

            /// <summary>
            /// Creates a new instance of <see cref="MonteCarloStrategySettings{TGame, TBoard, TMove, TPlayer}"/> from the properties configured on this <see cref="Build"/>
            /// </summary>
            /// <returns>The new instance of <see cref="MonteCarloStrategySettings{TGame, TBoard, TMove, TPlayer}"/></returns>
            /// <exception cref="ArgumentNullException">Thrown if <see cref="Builder.PlayerComparer"/> or <see cref="Builder.Random"/> is <see langword="null"/></exception>
            public MonteCarloStrategySettings<TGame, TBoard, TMove, TPlayer> Build()
            {
                if (this.PlayerComparer == null)
                {
                    throw new ArgumentNullException(nameof(this.PlayerComparer));
                }

                if (this.Random == null)
                {
                    throw new ArgumentNullException(nameof(this.Random));
                }

                return new MonteCarloStrategySettings<TGame, TBoard, TMove, TPlayer>(this.PlayerComparer, this.Random);
            }
        }
    }
}
