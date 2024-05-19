﻿namespace Fx.Strategy
{
    using Fx.Game;
    using System.Linq;

    public sealed class MonteCarloStrategy<TGame, TBoard, TMove, TPlayer> : IStrategy<TGame, TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
    {
        private readonly TPlayer player;

        private readonly int decisionCount;

        private readonly IEqualityComparer<TPlayer> playerComparer;

        private readonly Random random;

        public MonteCarloStrategy(TPlayer player, int decisionCount, IEqualityComparer<TPlayer> playerComparer, Random random)
        {
            Ensure.NotNull(player, nameof(player));
            Ensure.NotNull(playerComparer, nameof(playerComparer));
            Ensure.NotNull(random, nameof(random));

            this.player = player;
            this.decisionCount = decisionCount;
            this.playerComparer = playerComparer;
            this.random = random;
        }

        public TMove SelectMove(TGame game)
        {
            var outcomes = new Dictionary<int, (double winCount, int sampledCount)>();
            var remainingDecisionsCount = this.decisionCount;
            var moves = game.Moves.ToList();
            while (remainingDecisionsCount > 0)
            {
                var sampledGame = SampleGame(game, moves);
                if (outcomes.TryGetValue(sampledGame.moveIndex, out var counts))
                {
                    outcomes[sampledGame.moveIndex] = (counts.winCount + sampledGame.winLoseDraw, counts.sampledCount + 1);
                }
                else
                {
                    outcomes[sampledGame.moveIndex] = (sampledGame.winLoseDraw, 1);
                }

                remainingDecisionsCount -= sampledGame.numberOfDecisions;
            }

            var bestOutcome = outcomes.MaxBy(outcome => (double)outcome.Value.winCount / outcome.Value.sampledCount);
            return moves[bestOutcome.Key];
        }

        private (int moveIndex, double winLoseDraw, int numberOfDecisions) SampleGame(TGame game, IReadOnlyList<TMove> moves)
        {
            var initialMoveIndex = random.Next(0, moves.Count);
            var initialMove = moves[initialMoveIndex];
            game = game.CommitMove(initialMove);
            var numberOfDecisions = 1;
            while (game.WinnersAndLosers == null)
            {
                var nextMoves = game.Moves.ToList();
                var nextMoveIndex = random.Next(0, nextMoves.Count);
                var nextMove = nextMoves[nextMoveIndex];
                game = game.CommitMove(nextMove);
                ++numberOfDecisions;
            }

            var winLoseDraw = game.WinnersAndLosers.Winners.Any() ? game.WinnersAndLosers.Winners.Contains(this.player, this.playerComparer) ? 1 : 0 : 0.5; //// TODO are these good values?
            return (initialMoveIndex, winLoseDraw, numberOfDecisions);
        }
    }
}
