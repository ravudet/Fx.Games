namespace Fx.Games.Driver
{
    using System;
    using Db.System.Collections.Generic;
    using Fx.Games.Displayer;
    using Fx.Games.Game;
    using Fx.Games.Strategy;

    /// <summary>
    /// Coordinates the playing of a game by leveraging the specific strategies assigned to each player
    /// </summary>
    /// <typeparam name="TGame">The type of the game that is being played</typeparam>
    /// <typeparam name="TBoard">The type of the board that the <typeparamref name="TGame"/> uses</typeparam>
    /// <typeparam name="TMove">The type of the moves that the <typeparamref name="TGame"/> uses</typeparam>
    /// <typeparam name="TPlayer">The type of the player that is playing the <typeparamref name="TGame"/></typeparam>
    /// <threadsafety static="true" instance="true"/>
    public sealed class Driver<TGame, TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer> where TPlayer : notnull
    {
        /// <summary>
        /// The strategy that is assigned to each player of the game
        /// </summary>
        private readonly IReadOnlyDictionary<TPlayer, IStrategy<TGame, TBoard, TMove, TPlayer>> strategies;

        /// <summary>
        /// The <see cref="IDisplayer{TGame, TBoard, TMove, TPlayer}"/> that represents the input/output interactions between a user and the game that this <see cref="Driver{TGame, TBoard, TMove, TPlayer}"/> coordinates
        /// </summary>
        private readonly IDisplayer<TGame, TBoard, TMove, TPlayer> displayer;

        /// <summary>
        /// Converts a <see cref="TPlayer"/> to a string for logging and error handling
        /// </summary>
        private readonly Func<TPlayer, string> playerTranscriber;

        /// <summary>
        /// Initializes a new instance of the <see cref="Driver{TGame, TBoard, TMove, TPlayer}"/> class.
        /// </summary>
        /// <param name="strategies">The strategy that is assigned to each player of the game</param>
        /// <param name="displayer">The <see cref="IDisplayer{TGame, TBoard, TMove, TPlayer}"/> that represents the input/output interactions between a user and the game that this <see cref="Driver{TGame, TBoard, TMove, TPlayer}"/> coordinates</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="strategies"/> or <paramref name="displayer"/> is <see langword="null"/></exception>
        public Driver(IReadOnlyDictionary<TPlayer, IStrategy<TGame, TBoard, TMove, TPlayer>> strategies, IDisplayer<TGame, TBoard, TMove, TPlayer> displayer)
            : this(strategies, displayer, player => $"{player}")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Driver{TGame, TBoard, TMove, TPlayer}"/> class.
        /// </summary>
        /// <param name="strategies">The strategy that is assigned to each player of the game</param>
        /// <param name="displayer">The <see cref="IDisplayer{TGame, TBoard, TMove, TPlayer}"/> that represents the input/output interactions between a user and the game that this <see cref="Driver{TGame, TBoard, TMove, TPlayer}"/> coordinates</param>
        /// <param name="playerTranscriber">Converts a <see cref="TPlayer"/> to a string for logging and error handling</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="strategies"/> or <paramref name="displayer"/> or <paramref name="playerTranscriber"/> is <see langword="null"/></exception>
        public Driver(IReadOnlyDictionary<TPlayer, IStrategy<TGame, TBoard, TMove, TPlayer>> strategies, IDisplayer<TGame, TBoard, TMove, TPlayer> displayer, Func<TPlayer, string> playerTranscriber)
        {
            if (strategies == null)
            {
                throw new ArgumentNullException(nameof(strategies));
            }

            if (displayer == null)
            {
                throw new ArgumentNullException(nameof(displayer));
            }

            if (playerTranscriber == null)
            {
                throw new ArgumentNullException(nameof(playerTranscriber));
            }

            this.strategies = strategies; //// TODO create a copy constructor extension
            this.displayer = displayer;
            this.playerTranscriber = playerTranscriber;
        }

        /// <summary>
        /// Coordinates playing <paramref name="game"/> by leveraging the configured <see cref="IStrategy{TGame, TBoard, TMove, TPlayer}"/>s asssigned to each player of the game
        /// </summary>
        /// <param name="game">The game to play</param>
        /// <returns>The final state of the played game</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="game"/> is <see langword="null"/></exception>
        /// <exception cref="PlayerNotFoundExeption">Thrown if <paramref name="game"/> has reached a state where a certain player is the current player, but there is no strategy configured for that player</exception>
        /// <exception cref="InvalidGameException">Thrown if a strategy is asked to select a move from a game with no valid moves</exception>
        /// <exception cref="IllegalMoveExeption">Thrown if a strategy selects a move that is not currently legal</exception>
        public TGame Run(TGame game)
        {
            if (game == null)
            {
                throw new ArgumentNullException(nameof(game));
            }

            while (!game.IsGameOver)
            {
                var currentPlayer = game.CurrentPlayer;
                if (!this.strategies.TryGetValue(currentPlayer, out var strategy))
                {
                    //// TODO the string interpolation only works because you normally use string for the player type
                    throw new PlayerNotFoundExeption($"Could not find player {this.playerTranscriber(currentPlayer)} in the configured strategies.");
                }

                this.displayer.DisplayBoard(game);
                this.displayer.DisplayAvailableMoves(game);
                var move = strategy.SelectMove(game);
                this.displayer.DisplaySelectedMove(move);
                game = game.CommitMove(move);
            }

            this.displayer.DisplayBoard(game);
            this.displayer.DisplayOutcome(game);
            return game;
        }
    }
}
