namespace Fx.Games.Driver
{
    using System;
    using Db.System.Collections.Generic;
    using Fx.Games.Displayer;
    using Fx.Games.Game;
    using Fx.Games.Strategy;

    /// <summary>
    /// Factories for <see cref="Driver{TGame, TBoard, TMove, TPlayer}"/>
    /// </summary>
    /// <threadsafety static="true"/>
    public static class Driver
    {
        /// <summary>
        /// Creates a new instance of <see cref="Driver{TGame, TBoard, TMove, TPlayer}"/>
        /// </summary>
        /// <typeparam name="TGame">The type of the game that is being played</typeparam>
        /// <typeparam name="TBoard">The type of the board that the <typeparamref name="TGame"/> uses</typeparam>
        /// <typeparam name="TMove">The type of the moves that the <typeparamref name="TGame"/> uses</typeparam>
        /// <typeparam name="TPlayer">The type of the player that is playing the <typeparamref name="TGame"/></typeparam>
        /// <param name="strategies">The strategy that is assigned to each player of the game</param>
        /// <param name="displayer">The <see cref="IDisplayer{TGame, TBoard, TMove, TPlayer}"/> that represents the input/output interactions between a user and the game that this <see cref="Driver{TGame, TBoard, TMove, TPlayer}"/> coordinates</param>
        /// <returns>The new instance of <see cref="Driver{TGame, TBoard, TMove, TPlayer}"/></returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="strategies"/> or <paramref name="displayer"/> is <see langword="null"/></exception>
        public static Driver<TGame, TBoard, TMove, TPlayer> Create<TGame, TBoard, TMove, TPlayer>(
            IReadOnlyDictionary<TPlayer, IStrategy<TGame, TBoard, TMove, TPlayer>> strategies,
            IDisplayer<TGame, TBoard, TMove, TPlayer> displayer)
            where TGame : IGame<TGame, TBoard, TMove, TPlayer> where TPlayer : notnull
        {
            if (strategies == null)
            {
                throw new ArgumentNullException(nameof(strategies));
            }

            if (displayer == null)
            {
                throw new ArgumentNullException(nameof(displayer));
            }

            return new Driver<TGame, TBoard, TMove, TPlayer>(strategies, displayer);
        }
    }
}
