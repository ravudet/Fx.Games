namespace Fx.Games.Game
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The win/lose/draw status of each player of a game
    /// </summary>
    /// <typeparam name="TPlayer">The type of the player that is playing the game</typeparam>
    /// <threadsafety instance="true"/>
    public sealed class WinnersAndLosers<TPlayer>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WinnersAndLosers{TPlayer}"/> class.
        /// </summary>
        /// <param name="winners">The players who have won the game</param>
        /// <param name="losers">The players who have lost the game</param>
        /// <param name="drawers">The players who have drawn the game</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="winners"/> or <paramref name="losers"/> or <paramref name="drawers"/> is <see langword="null"/></exception>
        public WinnersAndLosers(IEnumerable<TPlayer> winners, IEnumerable<TPlayer> losers, IEnumerable<TPlayer> drawers)
        {
            if (winners == null)
            {
                throw new ArgumentNullException(nameof(winners));
            }

            if (losers == null)
            {
                throw new ArgumentNullException(nameof(losers));
            }

            if (drawers == null)
            {
                throw new ArgumentNullException(nameof(drawers));
            }

            Winners = winners;
            Losers = losers;
            Drawers = drawers;
        }

        public void Deconstruct(out IEnumerable<TPlayer> winners, out IEnumerable<TPlayer> losers, out IEnumerable<TPlayer> drawers)
        {
            winners = Winners;
            losers = Losers;
            drawers = Drawers;
        }


        /// <summary>
        /// The players who have won the game
        /// </summary>
        public IEnumerable<TPlayer> Winners { get; }

        /// <summary>
        /// The players who have lost the game
        /// </summary>
        public IEnumerable<TPlayer> Losers { get; }

        /// <summary>
        /// The players who have drawn the game
        /// </summary>
        public IEnumerable<TPlayer> Drawers { get; }
    }


    public static partial class WinnersAndLosers
    {
        public static WinnersAndLosers<TPlayer> None<TPlayer>(TPlayer left, TPlayer right) => new WinnersAndLosers<TPlayer>(Array.Empty<TPlayer>(), Array.Empty<TPlayer>(), Array.Empty<TPlayer>());
        public static WinnersAndLosers<TPlayer> LeftWins<TPlayer>(TPlayer left, TPlayer right) => new WinnersAndLosers<TPlayer>(new[] { left }, new[] { right }, Array.Empty<TPlayer>());
        public static WinnersAndLosers<TPlayer> RightWins<TPlayer>(TPlayer left, TPlayer right) => new WinnersAndLosers<TPlayer>(new[] { right }, new[] { left }, Array.Empty<TPlayer>());
        public static WinnersAndLosers<TPlayer> Draw<TPlayer>(TPlayer left, TPlayer right) => new WinnersAndLosers<TPlayer>(Array.Empty<TPlayer>(), Array.Empty<TPlayer>(), new[] { left, right });
    }
}
