namespace Fx.Games.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using Fx.Games.Game;

    /// <summary>
    /// A <see cref="IStrategy{TGame, TBoard, TMove, TPlayer}"/> that runs a <see href="https://en.wikipedia.org/wiki/Monte_Carlo_method">Monte Carlo simulation</see> to select the next best move to make
    /// </summary>
    /// <typeparam name="TGame">The type of the game that the strategy can play</typeparam>
    /// <typeparam name="TBoard">The type of the board that the <typeparamref name="TGame"/> uses</typeparam>
    /// <typeparam name="TMove">The type of the moves that the <typeparamref name="TGame"/> uses</typeparam>
    /// <typeparam name="TPlayer">The type of the player that is playing the <typeparamref name="TGame"/></typeparam>
    public sealed class MonteCarloStrategy<TGame, TBoard, TMove, TPlayer> : IStrategy<TGame, TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        /// <summary>
        /// The player of the <see cref="TGame"/> that is using the Monte Carlo method
        /// </summary>
        private readonly TPlayer player;

        /// <summary>
        /// The maximum number of "decisions" that the strategy should use when running the simulation
        /// </summary>
        private readonly int maxDecisionCount;

        /// <summary>
        /// A <see cref="IEqualityComparer{T}"/> that compares players of the game
        /// </summary>
        private readonly IEqualityComparer<TPlayer> playerComparer;

        /// <summary>
        /// The distribution to use when selecting random moves when the strategy simulates a game
        /// </summary>
        private readonly Random random;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonteCarloStrategy{TGame, TBoard, TMove, TPlayer}"/> class
        /// </summary>
        /// <param name="player">The player of the <see cref="TGame"/> that is using the Monte Carlo method</param>
        /// <param name="maxDecisionCount">The maximum number of "decisions" that the strategy should use when running the simulation</param>
        /// <param name="settings">The settings to use to configure the strategy</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="settings"/> is <see langword="null"/></exception>
        public MonteCarloStrategy(TPlayer player, int maxDecisionCount, MonteCarloStrategySettings<TGame, TBoard, TMove, TPlayer> settings)
        {
            // we are not checking 'player' because it's possible that the caller is legitimately using 'null' for a TPlayer of the game
            // we are not checking maxDecisionCount because nothing technically goes wrong if a negative or 0 value is provided, the simulation will just not actually run; this might be wrong from a usability standpoint
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            this.player = player;
            this.maxDecisionCount = maxDecisionCount;
            this.playerComparer = settings.PlayerComparer;
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

            var outcomes = new Dictionary<int, (double winLoseDraw, int sampledCount)>();
            var remainingDecisionCount = this.maxDecisionCount;

            while (remainingDecisionCount > 0)
            {
                var sampledGame = SampleGame(game, moves);
                if (outcomes.TryGetValue(sampledGame.moveIndex, out var counts))
                {
                    outcomes[sampledGame.moveIndex] = (counts.winLoseDraw + sampledGame.winLoseDraw, counts.sampledCount + 1);
                }
                else
                {
                    outcomes[sampledGame.moveIndex] = (sampledGame.winLoseDraw, 1);
                }

                remainingDecisionCount -= sampledGame.numberOfDecisions;
            }

            var bestOutcome = outcomes.MaxBy(outcome => (double)outcome.Value.winLoseDraw / outcome.Value.sampledCount);
            return moves[bestOutcome.Key];
        }

        /// <summary>
        /// Plays a random instance of <paramref name="game"/> by traversing one path of the decision tree
        /// </summary>
        /// <param name="game">The game to play an instance of; assumed to not be <see langword="null"/></param>
        /// <param name="moves">The moves that are currently available for the game; this is no different than <see cref="IGame{TGame, TBoard, TMove, TPlayer}.Moves"/> and is done purely for performance reasons; assumed to not be <see langword="null"/></param>
        /// <returns>
        /// moveIndex: The index in <paramref name="moves"/> of the first move that was selected for the simulated game
        /// winLoseDraw: 1 if <see cref="player"/> won the simulation, 0 if <see cref="player"/> lost the simulation, or 0.5 if the player was a drawer in the simulation
        /// numberOfDecisions: the number of decisions that were needed to run the simulation
        /// </returns>
        private (int moveIndex, double winLoseDraw, int numberOfDecisions) SampleGame(TGame game, IReadOnlyList<TMove> moves)
        {
            var initialMoveIndex = random.Next(0, moves.Count);
            var initialMove = moves[initialMoveIndex];
            game = game.CommitMove(initialMove);

            var numberOfDecisions = 1;
            while (!game.IsGameOver)
            {
                var nextMoves = game.Moves.ToList();
                var nextMoveIndex = random.Next(0, nextMoves.Count);
                var nextMove = nextMoves[nextMoveIndex];
                game = game.CommitMove(nextMove);
                ++numberOfDecisions;
            }

            var winLoseDraw = game.WinnersAndLosers.Winners.Contains(this.player, this.playerComparer) ? 1 : game.WinnersAndLosers.Losers.Contains(this.player, this.playerComparer) ? 0 : 0.5;
            return (initialMoveIndex, winLoseDraw, numberOfDecisions);
        }
    }
}
