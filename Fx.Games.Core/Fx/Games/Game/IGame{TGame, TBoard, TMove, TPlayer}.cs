namespace Fx.Games.Game
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// A turn-based game engine at a specific state within the game it represents
    /// TODO reconsider the names of basically everything
    /// TODO what about exceptions for cases where the game is implemented ona remote server?
    /// </summary>
    /// <typeparam name="TGame">The type of the game that is being represented</typeparam>
    /// <typeparam name="TBoard">The type of the board that the <typeparamref name="TGame"/> uses</typeparam>
    /// <typeparam name="TMove">The type of the moves that the <typeparamref name="TGame"/> uses</typeparam>
    /// <typeparam name="TPlayer">The type of the player that is playing the <typeparamref name="TGame"/></typeparam>
    /// <threadsafety instance="true"/>
    public interface IGame<out TGame, out TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        /// <summary>
        /// The <typeparamref name="TPlayer"/> whose turn it currently is
        /// </summary>
        TPlayer CurrentPlayer { get; }

        /// <summary>
        /// Commits <paramref name="move"/> to the current board state of the game
        /// </summary>
        /// <param name="move">The move that is being committed for the <see cref="CurrentPlayer"/></param>
        /// <returns>The board state of the game after <paramref name="move"/> has been committed</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="move"/> is <see langword="null"></exception>
        /// <exception cref="IllegalMoveExeption">Thrown if <paramref name="move"/> is not a legal move for the current board state of the game</exception>
        TGame CommitMove(TMove move);

        /// <summary>
        /// The legal moves in the current board state
        /// </summary>
        IEnumerable<TMove> Moves { get; }

        /// <summary>
        /// The current board state of the game
        /// </summary>
        TBoard Board { get; }

        /// <summary>
        /// The win/lose/draw status of each of the players of the game as of the current state
        /// </summary>
        WinnersAndLosers<TPlayer> WinnersAndLosers { get; }

        /// <summary>
        /// Whether or not the game is over
        /// </summary>
        bool IsGameOver { get; }
    }
}
