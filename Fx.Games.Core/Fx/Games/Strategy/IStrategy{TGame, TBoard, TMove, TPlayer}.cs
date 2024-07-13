namespace Fx.Games.Strategy
{
    using System;
    using Fx.Games.Game;

    /// <summary>
    /// A strategy to employ when playing a <see cref="IGame{TGame, TBoard, TMove, TPlayer}"/>
    /// </summary>
    /// <typeparam name="TGame">The type of the game that is being played</typeparam>
    /// <typeparam name="TBoard">The type of the board that the <typeparamref name="TGame"/> uses</typeparam>
    /// <typeparam name="TMove">The type of the moves that the <typeparamref name="TGame"/> uses</typeparam>
    /// <typeparam name="TPlayer">The type of the player that is playing the <typeparamref name="TGame"/></typeparam>
    /// <threadsafety instance="true"/>
    public interface IStrategy<in TGame, out TBoard, out TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        /// <summary>
        /// Determine which move of <paramref name="game"/> should be committed next
        /// </summary>
        /// <param name="game">The game which another move needs to be committed to</param>
        /// <returns>The move that should be committed to <paramref name="game"/></returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="game"/> is <see langword="null"/></exception>
        /// <exception cref="InvalidGameException">Thrown if <paramref name="game"/> has no valid moves for the current player</exception>
        TMove SelectMove(TGame game);
    }
}
