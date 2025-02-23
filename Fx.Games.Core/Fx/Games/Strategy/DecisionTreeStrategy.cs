namespace Fx.Games.Strategy
{
    using Fx.Distribution;
    using Fx.Games.Game;
    using Fx.Games.Game.Amazons;
    using Fx.Range;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Fx.Numerics;

    public interface IOperations
    {
        Natural Minus<TLeft, TRight>(TLeft left, TRight right) where TLeft : Natural, IGreaterThan<TRight> where TRight : Natural;

        Integer Minus(Natural left, Natural right);
    }

    public static class OperationsPlayground
    {
        public static void DoWork<TMinimum, TMaximum, TValue>(Fx.Range.Range<TMinimum, TMaximum, Natural, TValue> range, IOperations operations)
            where TMaximum : Natural, IGreaterThan<TMinimum>
            where TMinimum : Natural
        {

        }

        private sealed class CreateVisitor<TMinimum, TMaximum, TValue> : Fx.Range.RangeVisitor<TMinimum, TMaximum, TValue, Natural, Nothing, IOperations>
            where TMaximum : Natural, IGreaterThan<TMinimum>
            where TMinimum : Natural
        {
            private CreateVisitor()
            {
            }

            public static CreateVisitor<TMinimum, TMaximum, TValue> Instance { get; } = new CreateVisitor<TMinimum, TMaximum, TValue>();

            protected internal override Nothing Accept<TIntermediate, TMaximum2, TPreviousRange2>(Fx.Range.IntermediateSegment<TMinimum, TIntermediate, TMaximum2, TPreviousRange2, Natural, TValue> node, IOperations context)
            {
                var difference = context.Minus(node.Maximum, (node.PreviousRange as Fx.Range.Range<TMinimum, TMaximum, Natural, TValue>.StartingSegment)!.Minimum);

                var otherDifference = context.Minus((node.PreviousRange as Fx.Range.Range<TMinimum, TMaximum, Natural, TValue>.StartingSegment)!.Minimum, node.Maximum);

                return default;

                /*var globalMaximum = context ?? node.Maximmum;

                var nested = CreateVisitor<TMinimum, TIntermediate>.Instance.Visit(node.PreviousRange, globalMaximum);

                return (new SegmentV2<TValue, TNumeric>(nested.GlobalMinimum, globalMaximum, nested.NestedMaximum, node.Maximmum, node.Value, nested.Segment), node.Maximmum, nested.GlobalMinimum);*/
            }

            protected internal override Nothing Accept(Fx.Range.Range<TMinimum, TMaximum, Natural, TValue>.StartingSegment node, IOperations context)
            {
                return default;
                /*return (new SegmentV2<TValue, TNumeric>(node.Minimum, context!, node.Minimum, node.Maximum, node.Value, null), node.Maximum, node.Minimum);*/
            }
        }
    }

    public sealed class DecisionTreeStrategy<TGame, TBoard, TMove, TPlayer, TDistribution, TMinimum, TMaximum> : IStrategy<TGame, TBoard, TMove, TPlayer, TDistribution> where TGame : IGame<TGame, TBoard, TMove, TPlayer, TDistribution> where TDistribution : IDistribution<TGame> where TMinimum : Natural where TMaximum : Natural, IGreaterThan<TMinimum>
    {
        private readonly TPlayer desiredWinner;

        private readonly Func<TDistribution, SegmentV2<TGame, Natural>> weightedDistributionAdapter;

        private readonly IEqualityComparer<TPlayer> playerComparer;

        private readonly double drawWeight;

        public DecisionTreeStrategy(
            TPlayer desiredWinner, 
            Func<TDistribution, SegmentV2<TGame, Natural>> weightedDistributionAdapter,
            IEqualityComparer<TPlayer> playerComparer, 
            double drawWeight)
        {
            this.desiredWinner = desiredWinner;
            this.weightedDistributionAdapter = weightedDistributionAdapter;
            this.playerComparer = playerComparer;
            this.drawWeight = drawWeight; //// TODO parameterize all weights? //// TODO use settings for everything except `desiredWinner`
        }

        private TMove DoWork(TGame game)
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
            var weights = portions.ToWeights();

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
