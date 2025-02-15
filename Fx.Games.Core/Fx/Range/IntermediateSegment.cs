using Microsoft.Win32.SafeHandles;
using System.Net.NetworkInformation;
using System.Threading;
using System.Xml.Linq;

namespace Fx.Range
{
    using Fx.Numerics;
    using Stash;

    public interface IRange<TMinimum, TMaximum, TValue> where TMaximum : IGreaterThan<TMinimum>
    {
        //// TODO use a base class for this if possible
        //// TODO add the lessthan stuff

        TValue Value { get; }
    }

    public sealed class StartingSegment<TMinimum, TMaximum, TValue> : Range<TMinimum, TMaximum, TValue>, IRange<TMinimum, TMaximum, TValue> where TMaximum : IGreaterThan<TMinimum>
    {
        public StartingSegment(TMinimum minimum, TMaximum maximum, TValue value)
        {
            Minimum = minimum;
            Maximum = maximum;
            Value = value;
        }

        public TMinimum Minimum { get; }
        public TMaximum Maximum { get; }
        public override TValue Value { get; }

        public IntermediateSegment<TMinimum, TMaximum, TNewMaximum, StartingSegment<TMinimum, TMaximum, TValue>, TValue> FollowedBy<TNewMaximum>(TNewMaximum newMaximum, TValue value) where TNewMaximum : Natural, IGreaterThan<TMaximum>, IGreaterThan<TMinimum>
        {
            //// TODO can you implement these methods using the visitor?
            return new IntermediateSegment<TMinimum, TMaximum, TNewMaximum, StartingSegment<TMinimum, TMaximum, TValue>, TValue>(
                this,
                newMaximum,
                value);
        }

        protected internal override TResult Dispatch<TResult, TContext>(RangeVisitor<TMinimum, TMaximum, TValue, TResult, TContext> visitor, TContext context)
        {
            return visitor.Accept(this, context);
        }
    }

    public sealed class IntermediateSegment<TMinimum, TIntermediate, TMaximum, TPreviousRange, TValue> : Range<TMinimum, TMaximum, TValue>, IRange<TMinimum, TMaximum, TValue> where TPreviousRange : Range<TMinimum, TIntermediate, TValue>, IRange<TMinimum, TIntermediate, TValue> where TIntermediate : IGreaterThan<TMinimum> where TMaximum : IGreaterThan<TIntermediate>, IGreaterThan<TMinimum>
    {
        public IntermediateSegment(TPreviousRange previousRange, TMaximum maximmum, TValue value)
        {
            PreviousRange = previousRange;
            Maximmum = maximmum;
            Value = value;
        }

        public TPreviousRange PreviousRange { get; }
        public TMaximum Maximmum { get; }
        public override TValue Value { get; }

        public IntermediateSegment<TMinimum, TMaximum, TNewMaximum, IntermediateSegment<TMinimum, TIntermediate, TMaximum, TPreviousRange, TValue>, TValue> FollowedBy<TNewMaximum>(TNewMaximum newMaximum, TValue value) where TNewMaximum : Natural, IGreaterThan<TMaximum>, IGreaterThan<TMinimum>
        {
            return new IntermediateSegment<TMinimum, TMaximum, TNewMaximum, IntermediateSegment<TMinimum, TIntermediate, TMaximum, TPreviousRange, TValue>, TValue>(this, newMaximum, value);
        }

        protected internal override TResult Dispatch<TResult, TContext>(RangeVisitor<TMinimum, TMaximum, TValue, TResult, TContext> visitor, TContext context)
        {
            return visitor.Accept(this, context);
        }
    }

    public static class Range
    {
        public static StartingSegment<TMinimum, TMaximum, TValue> Instance<TMinimum, TMaximum, TValue>(TMinimum minimum, TMaximum maximum, TValue value) where TMaximum : Natural, IGreaterThan<TMinimum> where TMinimum : Natural
        {
            return new StartingSegment<TMinimum, TMaximum, TValue>(minimum, maximum, value);
        }

        /*public static IntermediateSegment<TMinimum, TMaximum, TNewMaximum, StartingSegment<TMinimum, TMaximum>> FollowedBy<TMinimum, TMaximum, TNewMaximum>(this StartingSegment<TMinimum, TMaximum> startingSegment, TNewMaximum newMaximum) where TMinimum : ILessThan<TMaximum>, ILessThan<TNewMaximum> where TMaximum : ILessThan<TNewMaximum>
        {
            return new IntermediateSegment<TMinimum, TMaximum, TNewMaximum, StartingSegment<TMinimum, TMaximum>>(
                startingSegment,
                newMaximum);
        }*/

        /*public static IntermediateSegment<TMinimum, TIntermediate, TMaximum, TPreviousRange> FollowedBy<TMinimum, TIntermediate, TMaximum, TPreviousRange>(this TPreviousRange previousRange, TMaximum maximum) where TPreviousRange : IRange<TMinimum, TIntermediate>
        {
            return new IntermediateSegment<TMinimum, TIntermediate, TMaximum, TPreviousRange>(previousRange, maximum);
        }*/
    }

    public static class Driver
    {
        public static void DoWork()
        {
            //// TODO try an operation that replaces the "maximum" of some segment by a different value (e.g. replace `four` with `three`)
            var range = Range
                .Instance(Naturals._1, Naturals._3, "first")
                .FollowedBy(Naturals._4, "second")
                .FollowedBy(Naturals._7, "third")
                .FollowedBy(Naturals._8, "fourth");

            //// TODO would it be nice to have something like Range.Start(_1).FollowedBy(-3, "first").FollowedBy(_4, "second")...?
        }
    }
}
