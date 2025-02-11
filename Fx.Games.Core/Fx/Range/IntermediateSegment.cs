using Microsoft.Win32.SafeHandles;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Xml.Linq;

namespace Fx.Range
{
    using Fx;
    using Fx.Numerics;

    public static class RangeExtensions
    {
        public static IEnumerable<(double Weight, TValue Value)> ToWeights<TMinimum, TMaximum, TValue>(this Segment<TMinimum, TMaximum, TValue> intermediateSegment) where TMaximum : Natural, IGreaterThan<TMinimum> where TMinimum : Natural
        {
            return ToWeightsVisitor<TMinimum, TMaximum, TValue>
                .Instance
                .Visit(intermediateSegment, default)
                .Select(
                    weight => (((double)weight.Range) / (weight.Intermediate - weight.Minimum), weight.Value));
        }

        /// <summary>
        /// TODO i don't think you should need the type parameters of `segment` to get to the visitor
        /// 
        /// TODO imnplement the `sample` method in `weighteddistribution` to make sure you can actually compute everything you need to
        /// </summary>
        /// <typeparam name="TMinimum"></typeparam>
        /// <typeparam name="TMaximum"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        private sealed class ToWeightsVisitor<TMinimum, TMaximum, TValue> : Segment<TMinimum, TMaximum, TValue>.Visitor<IEnumerable<(uint Range, TValue Value, uint Minimum, uint Intermediate)>, Nothing> where TMaximum : IGreaterThan<TMinimum>
        {
            private ToWeightsVisitor()
            {
            }

            public static ToWeightsVisitor<TMinimum, TMaximum, TValue> Instance { get; } = new ToWeightsVisitor<TMinimum, TMaximum, TValue>();

            protected internal override IEnumerable<(uint Range, TValue Value, uint Minimum, uint Intermediate)> Accept<TMinimum2, TMaximum2>(StartingSegment<TMinimum2, TMaximum2, TValue> node, Nothing context)
            {
                yield return (node.Maximum.ToClr() - node.Minimum.ToClr(), node.Value, node.Minimum.ToClr(), node.Maximum.ToClr());
            }

            protected internal override IEnumerable<(uint Range, TValue Value, uint Minimum, uint Intermediate)> Accept<TMinimum2, TIntermediate2, TMaximum2, TPreviousRange2>(IntermediateSegment<TMinimum2, TIntermediate2, TMaximum2, TPreviousRange2, TValue> node, Nothing context)
            {
                var previousWeights = ToWeightsVisitor<TMinimum2, TMaximum2, TValue>.Instance.Visit(node, context);
                (uint Range, TValue Value, uint Minimum, uint Intermediate)? lastWeight = null;
                foreach (var previousWeight in previousWeights)
                {
                    lastWeight = previousWeight;
                    yield return previousWeight;
                }

                //// TODO fix the nullable stuff with `lastweight`
                yield return (node.Maximmum.ToClr() - lastWeight.Value.Intermediate, node.Value, lastWeight.Value.Minimum, node.Maximmum.ToClr());
            }
        }
    }

    public interface IRange<TMinimum, TMaximum, TValue> where TMaximum : IGreaterThan<TMinimum>
    {
        //// TODO use a base class for this if possible
        //// TODO add the lessthan stuff

        TValue Value { get; }
    }

    public abstract class Segment<TMinimum, TMaximum, TValue> : IRange<TMinimum, TMaximum, TValue> where TMaximum : IGreaterThan<TMinimum>
    {
        internal protected Segment()
        {
            //// TODO should be private, and the derived classes should be nested
        }

        protected abstract TResult Dispatch<TResult, TContext>(Visitor<TResult, TContext> visitor, TContext context);

        public abstract class Visitor<TResult, TContext>
        {
            public TResult Visit(Segment<TMinimum, TMaximum, TValue> node, TContext context)
            {
                return node.Dispatch(this, context);
            }

            protected internal abstract TResult Accept<TMinimum2, TMaximum2>(StartingSegment<TMinimum2, TMaximum2, TValue> node, TContext context) where TMinimum2 : Natural where TMaximum2 : Natural, IGreaterThan<TMinimum2>; //// TODO you shouldn't have to specify `naturals` here, it should be somehow in the derived type
            protected internal abstract TResult Accept<TMinimum2, TIntermediate2, TMaximum2, TPreviousRange2>(IntermediateSegment<TMinimum2, TIntermediate2, TMaximum2, TPreviousRange2, TValue> node, TContext context) where TPreviousRange2 : Segment<TMinimum2, TIntermediate2, TValue>, IRange<TMinimum2, TIntermediate2, TValue> where TIntermediate2 : Natural, IGreaterThan<TMinimum2> where TMaximum2 : Natural, IGreaterThan<TIntermediate2>, IGreaterThan<TMinimum2> where TMinimum2 : Natural;
        }

        public abstract TValue Value { get; }
    }

    public sealed class StartingSegment<TMinimum, TMaximum, TValue> : Segment<TMinimum, TMaximum, TValue>, IRange<TMinimum, TMaximum, TValue> where TMaximum : Natural, IGreaterThan<TMinimum> where TMinimum : Natural
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
            return new IntermediateSegment<TMinimum, TMaximum, TNewMaximum, StartingSegment<TMinimum, TMaximum, TValue>, TValue>(
                this,
                newMaximum,
                value);
        }

        protected override TResult Dispatch<TResult, TContext>(Visitor<TResult, TContext> visitor, TContext context)
        {
            return visitor.Accept(this, context);
        }
    }

    public sealed class IntermediateSegment<TMinimum, TIntermediate, TMaximum, TPreviousRange, TValue> : Segment<TMinimum, TMaximum, TValue>, IRange<TMinimum, TMaximum, TValue> where TPreviousRange : Segment<TMinimum, TIntermediate, TValue>, IRange<TMinimum, TIntermediate, TValue> where TIntermediate : Natural, IGreaterThan<TMinimum> where TMaximum : Natural, IGreaterThan<TIntermediate>, IGreaterThan<TMinimum> where TMinimum : Natural
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

        protected override TResult Dispatch<TResult, TContext>(Visitor<TResult, TContext> visitor, TContext context)
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
