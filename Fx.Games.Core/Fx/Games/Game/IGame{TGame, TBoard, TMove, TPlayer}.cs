namespace Fx.Games.Game
{
    using Fx.Distribution;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;

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
    public interface IGame<TGame, out TBoard, TMove, TPlayer, out TDistribution> where TGame : IGame<TGame, TBoard, TMove, TPlayer, TDistribution> where TDistribution : IDistribution<TGame> //// TODO for some reason you were going down the path of using idistribution<tvalue, tdistribution>; however, you can't use univariate if that's the case (or maybe you can, but i can't seem to figure out how)
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

        TDistribution ExploreMove(TMove move);

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

    public abstract class Naturals
    {
        private Naturals()
        {
        }

        public sealed class Zero : Naturals, ILessThanOne
        {
            private Zero()
            {
            }

            public static Zero Instance { get; } = new Zero();
        }

        public sealed class One : Naturals, ILessThanTwo
        {
            private One()
            {
            }

            public static One Instance { get; } = new One();
        }

        public sealed class Two : Naturals, ILessThanThree
        {
            private Two()
            {
            }

            public static Two Instance { get; } = new Two();
        }

        public sealed class Three : Naturals
        {
            private Three()
            {
            }

            public static Three Instance { get; } = new Three();
        }
    }

    public interface ILessThanOne : ILessThanTwo, ILessThan<Naturals.One>
    {
    }

    public interface ILessThanTwo : ILessThanThree, ILessThan<Naturals.Two>
    {
    }

    public interface ILessThanThree : ILessThan<Naturals.Three>
    {
    }

    public interface ILessThan<TNatural>
    {
    }

    public class Range<TNatural> where TNatural : Naturals //// TODO this should be "wholes"
    {
    }

    public abstract class InOrder<TTotal, TCurrent> where TTotal : Naturals where TCurrent : Naturals, ILessThan<TTotal>
    {
    }

    public sealed class Intermediate<TTotal, TCurrent, TNext, TTheRest> : InOrder<TTotal, TCurrent> where TTotal : Naturals where TCurrent : Naturals, ILessThan<TNext>, ILessThan<TTotal> where TTheRest : InOrder<TTotal, TNext> where TNext : Naturals, ILessThan<TTotal>
    {
    }

    public sealed class Last<TTotal, TCurrent> : InOrder<TTotal, TCurrent> where TTotal : Naturals where TCurrent : Naturals, ILessThan<TTotal>
    {
    }

    public static class InOrderPlayground
    {
        public static void DoWork()
        {
            new Intermediate<Naturals.Three, Naturals.Zero, Naturals.Two, Last<Naturals.Three, Naturals.Two>>();




            new Intermediate<Naturals.Three, Naturals.Zero, Naturals.One,
                Intermediate<Naturals.Three, Naturals.One, Naturals.Two,
                    Last<Naturals.Three, Naturals.Two>>>();
        }

        /*public static IEnumerable<int> ToValues<TTotal, TCurrent>(InOrder<TTotal, TCurrent> inOrder) where TTotal : Naturals where TCurrent : Naturals, ILessThan<TTotal>
        {
        }*/
    }


    public static class NewAttempt
    {
        public interface ILessThan<T>
        {
        }

        public interface INumericValue
        {
            /// <summary>
            /// TODO uint isn't inherently the best here...
            /// </summary>
            static abstract uint Value { get; } //// TODO because this is abstract, anyone can implement their own; this probably isn't desireable for your purposes
        }

        public interface IWholes : INumericValue
        {
            public interface IOne : IWholes, ILessThanTwo
            {
                static uint INumericValue.Value { get; } = 1;
            }

            public interface ILessThanOne : ILessThan<IOne>, ILessThanTwo
            {
            }

            public interface ITwo : IWholes, ILessThanThree
            {
                static uint INumericValue.Value { get; } = 2;
            }

            public interface ILessThanTwo : ILessThan<ITwo>, ILessThanThree
            {
            }

            public interface IThree : IWholes, ILessThanFour
            {
                static uint INumericValue.Value { get; } = 3;
            }

            public interface ILessThanThree : ILessThan<IThree>, ILessThanFour
            {
            }

            public interface IFour : IWholes
            {
                static uint INumericValue.Value { get; } = 4;
            }

            public interface ILessThanFour : ILessThan<IFour>
            {
            }
        }

        public interface INaturals : INumericValue
        {
            public interface IZero : INaturals, IWholes.ILessThanOne
            {
                static uint INumericValue.Value { get; } = 0;
            }

            public interface IWhole<TWhole> : INaturals where TWhole : IWholes //// TODO do this work correctly? looking at `foo` and `bar` below, it appears to work, but will you sometimes want to declare that `TFirst : Wholes` or anything like that? (like in `foo2` and `bar2`)
            {
                static uint INumericValue.Value { get; } = TWhole.Value;
            }
        }

        public abstract class Ordered<TStart, TCurrent, TEnd> where TStart : ILessThan<TCurrent>, ILessThan<TEnd> where TCurrent : ILessThan<TEnd>
        {
        }

        public sealed class Intermediate<TStart, TCurrent, TNext, TEnd> : Ordered<TStart, TCurrent, TEnd> where TStart : ILessThan<TCurrent>, INumericValue, ILessThan<TNext>, ILessThan<TEnd> where TCurrent : ILessThan<TNext>, ILessThan<TEnd>, INumericValue where TNext : ILessThan<TEnd>, INumericValue where TEnd : INumericValue
        {
            public Intermediate(Ordered<TStart, TNext, TEnd> theRest)
            {
            }
        }

        public sealed class Last<TStart, TCurrent, TEnd> : Ordered<TStart, TCurrent, TEnd> where TStart : ILessThan<TEnd>, ILessThan<TCurrent>, INumericValue where TCurrent : ILessThan<TEnd>, INumericValue where TEnd : INumericValue
        {
        }

        public interface IOrdered<TStart, TCurrent, TEnd> where TStart : ILessThan<TCurrent>, ILessThan<TEnd> where TCurrent : ILessThan<TEnd>
        {
            static abstract IEnumerable<double> Weights<TPrevious>() where TPrevious : ILessThan<TCurrent>, INumericValue; //// TODO because this is abstract, anyone can implement their own; this probably isn't desireable for your purposes
        }

        public interface IIntermediate<TStart, TCurrent, TNext, TEnd, TTheRest> : IOrdered<TStart, TCurrent, TEnd> where TStart : ILessThan<TCurrent>, INumericValue, ILessThan<TNext>, ILessThan<TEnd> where TCurrent : ILessThan<TNext>, ILessThan<TEnd>, INumericValue where TNext : ILessThan<TEnd>, INumericValue where TTheRest : IOrdered<TStart, TNext, TEnd> where TEnd : INumericValue
        {
            //// TODO do the generic type constraints need an igreaterthan interface?

            static IEnumerable<double> IOrdered<TStart, TCurrent, TEnd>.Weights<TPrevious>()
            {
                return TTheRest
                    .Weights<TCurrent>() //// TODO does this actually end up being lazy?
                    .Prepend(((double)(TCurrent.Value - TPrevious.Value)) / (TEnd.Value - TStart.Value));
            }
        }

        public interface ILast<TStart, TCurrent, TEnd> : IOrdered<TStart, TCurrent, TEnd> where TStart : ILessThan<TEnd>, ILessThan<TCurrent>, INumericValue where TCurrent : ILessThan<TEnd>, INumericValue where TEnd : INumericValue
        {
            static IEnumerable<double> IOrdered<TStart, TCurrent, TEnd>.Weights<TPrevious>()
            {
                yield return ((double)(TCurrent.Value - TPrevious.Value)) / (TEnd.Value - TStart.Value);
                yield return ((double)(TEnd.Value - TCurrent.Value)) / (TEnd.Value - TStart.Value);
            }
        }

        public static void CreateOrdered()
        {
            var weights = Weights<IIntermediate<INaturals.IZero, IWholes.IOne, IWholes.ITwo, IWholes.IThree,
                ILast<INaturals.IZero, IWholes.ITwo, IWholes.IThree>>, INaturals.IZero, IWholes.IOne, IWholes.IThree>();

            /*var otherWeights = Weights<IOrdered<INaturals.IZero, IWholes.IOne, IWholes.ITwo>, INaturals.IZero, IWholes.ITwo, IWholes.IThree>();*/
        }

        public static IEnumerable<double> Weights<TOrdered, TStart, TCurrent, TEnd>() where TOrdered : IOrdered<TStart, TCurrent, TEnd> where TStart : ILessThan<TCurrent>, INumericValue, ILessThan<TEnd> where TCurrent : ILessThan<TEnd>
        {
            return TOrdered.Weights<TStart>();
        }

        public static void Foo<TFirst, TSecond>() where TFirst : ILessThan<TSecond>
        {
        }

        public static void Bar()
        {
            Foo<INaturals.IZero, IWholes.IThree>();
        }

        /*public static void Foo2<TFirst, TSecond>() where TFirst : INaturals, ILessThan<TSecond> where TSecond : INaturals
        {
        }

        public static void Bar2()
        {
            Foo<INaturals.IZero, INaturals.IWhole<IWholes.IThree>>();
        }*/
    }

    public sealed class PortionV3<TTotal, TCurrent> where TTotal : Naturals where TCurrent : Naturals, ILessThan<TTotal>
    {
        public PortionV3(InOrder<TTotal, TCurrent> inOrder)
        {
        }
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

            //// TODO are there some double inaccuracies to check for?

            if (!weights.MoveNext())
            {
                return PortionV2.All(current.Item2);
            }
            else
            {
                return PortionV2.Some(current.Item2, (uint)((current.Item1 / remainingWeight) * uint.MaxValue), ConvertToPortion(weights, remainingWeight - current.Item1));
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
