namespace Fx.Games.Strategy
{
    using System;

    /// <summary>
    /// The settings used to instantiate a <see cref="RandomStrategy{TGame, TBoard, TMove, TPlayer}"/>
    /// </summary>
    /// <typeparam name="TGame">The type of the game that the strategy can play</typeparam>
    /// <typeparam name="TBoard">The type of the board that the <typeparamref name="TGame"/> uses</typeparam>
    /// <typeparam name="TMove">The type of the moves that the <typeparamref name="TGame"/> uses</typeparam>
    /// <typeparam name="TPlayer">The type of the player that is playing the <typeparamref name="TGame"/></typeparam>
    public sealed class RandomStrategySettings<TGame, TBoard, TMove, TPlayer>
    {
        /// <summary>
        /// Prevents the initialization of the <see cref="RandomStrategySettings{TGame, TBoard, TMove, TPlayer}"/> class
        /// </summary>
        /// <param name="random">The distribution to use when selecting a random move</param>
        private RandomStrategySettings(Random random)
        {
            Random = random;
        }

        /// <summary>
        /// The default instance of <see cref="RandomStrategySettings{TGame, TBoard, TMove, TPlayer}"/>
        /// </summary>
        public static RandomStrategySettings<TGame, TBoard, TMove, TPlayer> Default { get; } = new RandomStrategySettings<TGame, TBoard, TMove, TPlayer>(new Random());

        /// <summary>
        /// The distribution to use when selecting a random move
        /// </summary>
        public Random Random { get; }

        /// <summary>
        /// A builder for <see cref="RandomStrategySettings{TGame, TBoard, TMove, TPlayer}"/>
        /// </summary>
        public sealed class Builder
        {
            /// <summary>
            /// The distribution to use when selecting a random move
            /// </summary>
            public Random Random { get; set; } = Default.Random;

            /// <summary>
            /// Creates a new instance of <see cref="RandomStrategySettings{TGame, TBoard, TMove, TPlayer}"/> from the properties configued on this <see cref="Builder"/>
            /// </summary>
            /// <returns>The new instance of <see cref="RandomStrategySettings{TGame, TBoard, TMove, TPlayer}"/></returns>
            /// <exception cref="ArgumentNullException">Thrown if <see cref="Builder.Random"/> is <see langword="null"/></exception>
            public RandomStrategySettings<TGame, TBoard, TMove, TPlayer> Build()
            {
                if (this.Random == null)
                {
                    throw new ArgumentNullException(nameof(this.Random));
                }

                return new RandomStrategySettings<TGame, TBoard, TMove, TPlayer>(this.Random);
            }
        }
    }
}
