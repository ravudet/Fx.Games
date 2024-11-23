namespace Fx.Games.Game
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
    public interface IGame<TGame, out TBoard, TMove, TPlayer> where TGame : IGame<TGame, TBoard, TMove, TPlayer>
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

        ////IEnumerable<(double, TGame)> ExploreMove(TMove move);

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

    public sealed class Portion<TValue, TRemainder>
    {
        public Portion(TValue value, uint likelihood, TRemainder remainder)
        {
            this.Value = value;
            this.Likelihood = likelihood;
            this.Remainder = remainder;
        }

        public TValue Value { get; }
        public uint Likelihood { get; }
        public TRemainder Remainder { get; }
    }

    public abstract class PortionV2<TValue>
    {
        private PortionV2()
        {
        }

        public sealed class Some : PortionV2<TValue>
        {
            public Some(TValue value, uint likelihood, PortionV2<TValue> remainder)
            {
                this.Value = value;
                this.Likelihood = likelihood;
                this.Remainder = remainder;
            }

            public TValue Value { get; }
            public uint Likelihood { get; }
            public PortionV2<TValue> Remainder { get; }
        }

        public sealed class All : PortionV2<TValue>
        {
            public All(TValue value)
            {
                this.Value = value;
            }

            public TValue Value { get; }
        }
    }

    public static class PortionV2
    {
        public static PortionV2<TValue> Some<TValue>(TValue value, uint likelihood, PortionV2<TValue> remainder)
        {
            return new PortionV2<TValue>.Some(value, likelihood, remainder);
        }

        public static PortionV2<TValue> All<TValue>(TValue value)
        {
            return new PortionV2<TValue>.All(value);
        }

        public static TResult Visit<TValue, TResult, TContext>(
            this PortionV2<TValue> portion, 
            Func<PortionV2<TValue>.Some, TContext, TResult> someDispatch, 
            Func<PortionV2<TValue>.All, TContext, TResult> allDispatch,
            TContext context)
        {
            if (portion is PortionV2<TValue>.Some some)
            {
                return someDispatch(some, context);
            }
            else if (portion is PortionV2<TValue>.All all)
            {
                return allDispatch(all, context);
            }
            else
            {
                throw new Exception("TODO use visitor");
            }
        }
    }

    public static class PortionV2Playground
    {
        public static IEnumerable<(double, TValue)> ConvertToWeights<TValue>(PortionV2<TValue> portion)
        {
            return portion.Visit(SomeDispatch, AllDispatch, 1.0);
        }

        private static IEnumerable<(double, TValue)> SomeDispatch<TValue>(PortionV2<TValue>.Some some, double remainingWeight)
        {
            var weight = ((double)some.Likelihood) / uint.MaxValue;
            var value = some.Value;
            return some.Remainder.Visit(SomeDispatch, AllDispatch, remainingWeight * (1.0 - weight)).Prepend((weight * remainingWeight, value));
        }

        private static IEnumerable<(double, TValue)> AllDispatch<TValue>(PortionV2<TValue>.All all, double remainingWeight)
        {
            var weight = 1.0;
            var value = all.Value;
            
            return new[] { (weight * remainingWeight, value) };
        }

        public static PortionV2<TValue> ConvertToPortion<TValue>(IEnumerable<(double, TValue)> weights)
        {
            using (var enumerator = weights.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    throw new Exception("tODO empty");
                }

                return ConvertToPortion(enumerator, 1.0);
            }
        }

        private static PortionV2<TValue> ConvertToPortion<TValue>(IEnumerator<(double, TValue)> weights, double remainingWeight)
        {
            var current = weights.Current;

            var newRemainder = remainingWeight - current.Item1;
            var normalizedWeight = current.Item1 / (1.0 - remainingWeight);
            if (newRemainder < 0)
            {
                throw new Exception("TODO weights are 100");
            }

            if (!weights.MoveNext())
            {
                if (newRemainder != 0)
                {
                    //// TODO
                }

                return PortionV2.All(current.Item2);
            }
            else
            {
                return PortionV2.Some(current.Item2, (uint)(uint.MaxValue * normalizedWeight), ConvertToPortion(weights, newRemainder));
            }
        }
    }

    /*public static class SpikeExtensions
    {
        public static TGame CommitMove<TGame, TBoard, TMove, TPlayer>(
            this IGame<TGame, TBoard, TMove, TPlayer> game,
            TMove move)
            where TGame : IGame<TGame, TBoard, TMove, TPlayer>
        {
            var random = new Random();
            var next = random.NextDouble();
            using (var enumerator = game.ExploreMove(move).GetEnumerator())
            {
                while (enumerator.MoveNext() && next > 0)
                {
                    next -= enumerator.Current.Item1;
                }

                return enumerator.Current.Item2;
            }
        }
    }*/
}
