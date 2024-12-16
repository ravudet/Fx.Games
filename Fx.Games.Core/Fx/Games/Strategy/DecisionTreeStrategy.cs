namespace Fx.Games.Strategy
{
    using Fx.Distribution;
    using Fx.Games.Game;
    using Fx.Games.Game.Amazons;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class DecisionTreeStrategy<TGame, TBoard, TMove, TPlayer, TDistribution> : IStrategy<TGame, TBoard, TMove, TPlayer, TDistribution> where TGame : IGame<TGame, TBoard, TMove, TPlayer, TDistribution> where TDistribution : IDistribution<TGame>
    {
        private readonly TPlayer desiredWinner;

        private readonly Func<TDistribution, PortionV2<TGame>> weightedDistributionAdapter;

        private readonly IEqualityComparer<TPlayer> playerComparer;

        private readonly double drawWeight;

        public DecisionTreeStrategy(
            TPlayer desiredWinner, 
            Func<TDistribution, PortionV2<TGame>> weightedDistributionAdapter,
            IEqualityComparer<TPlayer> playerComparer, 
            double drawWeight)
        {
            this.desiredWinner = desiredWinner;
            this.weightedDistributionAdapter = weightedDistributionAdapter;
            this.playerComparer = playerComparer;
            this.drawWeight = drawWeight; //// TODO parameterize all weights? //// TODO use settings for everything except `desiredWinner`
        }

        public TMove DoWork(TGame game)
        {
            return game
                .Moves
                .Select(move => (move, game.ExploreMove(move)))
                .Select(distribution => (distribution.move, ComputeOutcome(distribution.Item2)))
                .MaxBy(outcome => outcome.Item2, new OutcomeComparer(this.drawWeight))
                .move;
        }

        private Outcome ComputeOutcome(TDistribution distribution)
        {
            var portions = this.weightedDistributionAdapter(distribution);
            var weights = PortionV2Playground.ConvertToWeights(portions);

            return AverageOfOutcomes(weights.Select(weight => (weight.Item1, ComputeOutcome(weight.Item2))));
        }

        private static Outcome AverageOfOutcomes(IEnumerable<(double Weight, Outcome Outcome)> outcomes)
        {
            //// TODO this would be great to levarege mixins
            var allWins = true;
            var allLosses = true;
            var allDraws = true;
            var probability = 0.0;
            var count = 0;
            foreach (var outcome in outcomes)
            {
                if (outcome.Outcome is Outcome.Win)
                {
                    probability += outcome.Weight * 1.0;
                    allLosses = false;
                    allDraws = false;
                }
                else if (outcome.Outcome is Outcome.Loss)
                {
                    probability += outcome.Weight * -1.0;
                    allWins = false;
                    allDraws = false;
                }
                else if (outcome.Outcome is Outcome.Draw)
                {
                    probability += outcome.Weight * 0.0; //// TODO parameterize all of these literals
                    allWins = false;
                    allLosses = false;
                }
                else if (outcome.Outcome is Outcome.Probability likelihood)
                {
                    probability += outcome.Weight * likelihood.Liklihood;
                    allWins = false;
                    allLosses = false;
                    allDraws = false;
                }
                else
                {
                    throw new Exception("TODO visitor");
                }

                ++count;
            }

            if (allWins)
            {
                return Outcome.Win.Instance;
            }

            if (allLosses)
            {
                return Outcome.Loss.Instance;
            }

            if (allDraws)
            {
                return Outcome.Draw.Instance;
            }

            if (count == 0)
            {
                throw new Exception("TODO no outcomes to average");
            }

            return new Outcome.Probability(probability /* TODO do you need the count if you're already weighted? / count*/);
        }

        private Outcome ComputeOutcome(TGame game)
        {
            if (game.IsGameOver)
            {
                if (game.WinnersAndLosers.Winners.Contains(this.desiredWinner, this.playerComparer))
                {
                    return Outcome.Win.Instance;
                }
                else if (game.WinnersAndLosers.Losers.Contains(this.desiredWinner, this.playerComparer))
                {
                    return Outcome.Loss.Instance;
                }
                else if (game.WinnersAndLosers.Drawers.Contains(this.desiredWinner, this.playerComparer))
                {
                    return Outcome.Draw.Instance;
                }
                else
                {
                    throw new Exception("TODO bad game implementation or desiredWinner wasn't a player of the game");
                }
            }

            var moves = game.Moves.ToList();
            var outcomes = moves
                .Select(move => game.ExploreMove(move))
                .Select(distribution => ComputeOutcome(distribution))
                .Select(outcome => (1.0 / moves.Count, outcome));
            return AverageOfOutcomes(outcomes);
        }

        public TMove SelectMove(TGame game)
        {
            System.Console.WriteLine(DateTime.UtcNow);
            var move = DoWork(game);
            System.Console.WriteLine(DateTime.UtcNow);
            return move;
        }

        private Outcome PlayMoves(TGame game, TMove move)
        {
            var newGames = game.ExploreMove(move);
            var newGameProbabilities = PortionV2Playground.ConvertToWeights(this.weightedDistributionAdapter(newGames)).ToList();

            var allWins = true;
            var allLosses = true;
            var allDraws = true;
            var probability = 0.0;
            foreach (var newGameProbability in newGameProbabilities)
            {
                var outcome = PlayMove(newGameProbability.Item2);
                if (outcome is Outcome.Win)
                {
                    allLosses = false;
                    allDraws = false;

                    probability += newGameProbability.Item1 * 1.0;
                }
                else if (outcome is Outcome.Loss)
                {
                    allWins = false;
                    allDraws = false;

                    probability += newGameProbability.Item1 * -1.0;
                }
                else if (outcome is Outcome.Draw)
                {
                    allWins = false;
                    allLosses = false;

                    probability += newGameProbability.Item1 * this.drawWeight;
                }
                else if (outcome is Outcome.Probability liklihood)
                {
                    allWins = false;
                    allLosses = false;
                    allDraws = false;

                    probability += newGameProbability.Item1 * liklihood.Liklihood;
                }
                else
                {
                    throw new System.Exception("TODO use visitor");
                }
            }

            if (allWins)
            {
                return Outcome.Win.Instance;
            }

            if (allLosses)
            {
                return Outcome.Loss.Instance;
            }

            if (allDraws)
            {
                return Outcome.Draw.Instance;
            }

            return new Outcome.Probability(probability / newGameProbabilities.Count);
        }

        private Outcome PlayMove(TGame game)
        {
            if (game.IsGameOver)
            {
                if (game.WinnersAndLosers.Winners.Contains(this.desiredWinner, this.playerComparer))
                {
                    return Outcome.Win.Instance;
                }
                else if (game.WinnersAndLosers.Losers.Contains(this.desiredWinner, this.playerComparer))
                {
                    return Outcome.Loss.Instance;
                }
                else if (game.WinnersAndLosers.Drawers.Contains(this.desiredWinner, this.playerComparer))
                {
                    return Outcome.Draw.Instance;
                }
                else
                {
                    throw new System.Exception("TODO the game engine lost track of the player...");
                }
            }

            var moves = game.Moves.ToList(); //// TODO something like queryresult could be used here where, when done enumerating, we know the count
            var allWins = true;
            var allLosses = true;
            var allDraws = true;
            var probability = 0.0;
            foreach (var move in moves)
            {
                var outcome = PlayMoves(game, move);
                if (outcome is Outcome.Win)
                {
                    allLosses = false;
                    allDraws = false;

                    probability += 1.0;
                }
                else if (outcome is Outcome.Loss)
                {
                    allWins = false;
                    allDraws = false;

                    probability += -1.0;
                }
                else if (outcome is Outcome.Draw)
                {
                    allWins = false;
                    allLosses = false;

                    probability += this.drawWeight;
                }
                else if (outcome is Outcome.Probability liklihood)
                {
                    allWins = false;
                    allLosses = false;
                    allDraws = false;

                    probability += liklihood.Liklihood;
                }
                else
                {
                    throw new System.Exception("TODO use visitor");
                }
            }

            if (allWins)
            {
                return Outcome.Win.Instance;
            }

            if (allLosses)
            {
                return Outcome.Loss.Instance;
            }

            if (allDraws)
            {
                return Outcome.Draw.Instance;
            }

            return new Outcome.Probability(probability / moves.Count);
        }

        private sealed class OutcomeComparer : IComparer<Outcome>
        {
            private readonly double drawWeight;

            public OutcomeComparer(double drawWeight)
            {
                this.drawWeight = drawWeight;
            }

            public int Compare(Outcome? x, Outcome? y)
            {
                if (x == null)
                {
                    return -1;
                }

                if (y == null)
                {
                    return 1;
                }

                if (object.ReferenceEquals(x, y))
                {
                    return 0;
                }

                if (x is Outcome.Win)
                {
                    return 1;
                }

                if (x is Outcome.Loss)
                {
                    return -1;
                }

                if (y is Outcome.Win)
                {
                    return -1;
                }

                if (y is Outcome.Loss)
                {
                    return 1;
                }

                double xProbability;
                if (x is Outcome.Draw)
                {
                    xProbability = this.drawWeight;
                }
                else if (x is Outcome.Probability probability)
                {
                    xProbability = probability.Liklihood;
                }
                else
                {
                    throw new System.Exception("tODO use visitor");
                }

                double yProbability;
                if (y is Outcome.Draw)
                {
                    yProbability = this.drawWeight;
                }
                else if (y is Outcome.Probability probability)
                {
                    yProbability = probability.Liklihood;
                }
                else
                {
                    throw new System.Exception("tODO use visitor");
                }

                return xProbability.CompareTo(yProbability);
            }
        }

        private abstract class Outcome
        {
            private Outcome()
            {
            }

            public sealed class Win : Outcome
            {
                private Win()
                {
                }

                public static Win Instance { get; } = new Win();
            }

            public sealed class Draw : Outcome
            {
                private Draw()
                {
                }

                public static Draw Instance { get; } = new Draw();
            }

            public sealed class Loss : Outcome
            {
                private Loss()
                {
                }

                public static Loss Instance { get; } = new Loss();
            }

            public sealed class Probability : Outcome
            {
                public Probability(double liklihood)
                {
                    Liklihood = liklihood;
                }

                public double Liklihood { get; }
            }
        }
    }
}
