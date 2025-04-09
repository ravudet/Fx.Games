namespace Fx.Range
{
    using System.Collections.Generic;
    using System.Linq;

    using Fx;
    using Fx.Interval;
    using Fx.Numerics;

    public static class RangeExtensions
    {
        public static IEnumerable<(double Weight, TValue Value)> ToWeights<TMinimum, TMaximum, TValue>(this Fx.Interval.Interval<TMinimum, TMaximum, Natural, TValue> intermediateSegment) where TMaximum : Natural, IGreaterThan<TMinimum> where TMinimum : Natural
        {
            //// TODO instead of implementing a visitor here, if you just create a `segmentv2`, you can delegate to its `ToWeights` extension; keeping the implementation here for now because it's a good example for the range visitor when you're recompiling after a re-design
            return ToWeightsVisitor2<TMinimum, TMaximum, TValue>
                .Instance
                .Visit(intermediateSegment, new Nothing())
                .Select(
                    weight => (((double)weight.Range) / (weight.Intermediate - weight.Minimum), weight.Value));
        }

        private sealed class ToWeightsVisitor2<TMinimum, TMaximum, TValue> : Fx.Interval.IntervalVisitor<TMinimum, TMaximum, TValue, Natural, IEnumerable<(uint Range, TValue Value, uint Minimum, uint Intermediate)>, Nothing> where TMaximum : Natural, IGreaterThan<TMinimum> where TMinimum : Natural //// TODO i think you can use a numerics interface instead of `natural`
        {
            private ToWeightsVisitor2()
            {
            }

            public static ToWeightsVisitor2<TMinimum, TMaximum, TValue> Instance { get; } = new ToWeightsVisitor2<TMinimum, TMaximum, TValue>();

            protected internal override IEnumerable<(uint Range, TValue Value, uint Minimum, uint Intermediate)> Accept(Interval<TMinimum, TMaximum, Natural, TValue>.Mesh node, Nothing context)
            {
                yield return (node.Maximum.ToClr() - node.Minimum.ToClr(), node.Value, node.Minimum.ToClr(), node.Maximum.ToClr());
            }

            protected internal override IEnumerable<(uint Range, TValue Value, uint Minimum, uint Intermediate)> Accept<TMaximum2, TNewMaximum, TSubInterval2>(Interval<TMinimum, TMaximum2, Natural, TValue>.Partition<TNewMaximum, TSubInterval2> node, Nothing context)
            {
                var previousWeights = ToWeightsVisitor2<TMinimum, TMaximum2, TValue>.Instance.Visit(node.SubInterval, context); //// TODO you stole this from other code that thing might have a bug; the code you stole it from just passed `node` in, in case it actually wasn't a bug
                (uint Range, TValue Value, uint Minimum, uint Intermediate)? lastWeight = null;
                foreach (var previousWeight in previousWeights)
                {
                    lastWeight = previousWeight;
                    yield return previousWeight;
                }

                //// TODO fix the nullable stuff with `lastweight`
                yield return (node.Maximum.ToClr() - lastWeight.Value.Intermediate, node.Value, lastWeight.Value.Minimum, node.Maximum.ToClr());
            }
        }

        public static IEnumerable<(double Weight, TValue Value)> ToWeights<TMinimum, TMaximum, TValue>(this Range<TMinimum, TMaximum, Natural, TValue> intermediateSegment) where TMaximum : Natural, IGreaterThan<TMinimum> where TMinimum : Natural
        {
            //// TODO instead of implementing a visitor here, if you just create a `segmentv2`, you can delegate to its `ToWeights` extension; keeping the implementation here for now because it's a good example for the range visitor when you're recompiling after a re-design
            return ToWeightsVisitor<TMinimum, TMaximum, TValue>
                .Instance
                .Visit(intermediateSegment, default)
                .Select(
                    weight => (((double)weight.Range) / (weight.Intermediate - weight.Minimum), weight.Value));
        }

        /// <summary>
        /// 
        /// 
        /// TODO imnplement the `sample` method in `weighteddistribution` to make sure you can actually compute everything you need to
        /// </summary>
        /// <typeparam name="TMinimum"></typeparam>
        /// <typeparam name="TMaximum"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        private sealed class ToWeightsVisitor<TMinimum, TMaximum, TValue> : RangeVisitor<TMinimum, TMaximum, TValue, Natural, IEnumerable<(uint Range, TValue Value, uint Minimum, uint Intermediate)>, Nothing> where TMaximum : Natural, IGreaterThan<TMinimum> where TMinimum : Natural //// TODO i think you can use a numerics interface instead of `natural`
        {
            private ToWeightsVisitor()
            {
            }

            public static ToWeightsVisitor<TMinimum, TMaximum, TValue> Instance { get; } = new ToWeightsVisitor<TMinimum, TMaximum, TValue>();

            protected internal override IEnumerable<(uint Range, TValue Value, uint Minimum, uint Intermediate)> Accept<TIntermediate, TMaximum2, TPreviousRange2>(IntermediateSegment<TMinimum, TIntermediate, TMaximum2, TPreviousRange2, Natural, TValue> node, Nothing context)
            {
                var previousWeights = ToWeightsVisitor<TMinimum, TMaximum2, TValue>.Instance.Visit(node, context); //// TODO is this a bug? should it be `node.PreviousRange`?
                (uint Range, TValue Value, uint Minimum, uint Intermediate)? lastWeight = null;
                foreach (var previousWeight in previousWeights)
                {
                    lastWeight = previousWeight;
                    yield return previousWeight;
                }

                //// TODO fix the nullable stuff with `lastweight`
                yield return (node.Maximum.ToClr() - lastWeight.Value.Intermediate, node.Value, lastWeight.Value.Minimum, node.Maximum.ToClr());
            }

            protected internal override IEnumerable<(uint Range, TValue Value, uint Minimum, uint Intermediate)> Accept(Range<TMinimum, TMaximum, Natural, TValue>.StartingSegment node, Nothing context)
            {
                yield return (node.Maximum.ToClr() - node.Minimum.ToClr(), node.Value, node.Minimum.ToClr(), node.Maximum.ToClr());
            }
        }

        public static IntermediateSegment
            <
                TMinimum, 
                TMaximum, 
                TNewMaximum, 
                IntermediateSegment
                    <
                        TMinimum, 
                        TIntermediate, 
                        TMaximum, 
                        TPreviousRange, 
                        TNumeric, 
                        TValue
                    >, 
                TNumeric, 
                TValue
            > 
            FollowedBy<TMinimum, TMaximum, TNewMaximum, TIntermediate, TPreviousRange, TNumeric, TValue>(
                this IntermediateSegment<TMinimum, TIntermediate, TMaximum, TPreviousRange, TNumeric, TValue> segment,
                TNewMaximum newMaximum,
                TValue value) 
            where TMinimum : TNumeric 
            where TIntermediate : TNumeric, IGreaterThan<TMinimum>
            where TMaximum : TNumeric, IGreaterThan<TIntermediate>, IGreaterThan<TMinimum>
            where TNewMaximum : TNumeric, IGreaterThan<TMaximum>, IGreaterThan<TMinimum>
            where TPreviousRange : Range<TMinimum, TIntermediate, TNumeric, TValue>
        {
            return new IntermediateSegment<TMinimum, TMaximum, TNewMaximum, IntermediateSegment<TMinimum, TIntermediate, TMaximum, TPreviousRange, TNumeric, TValue>, TNumeric, TValue>(segment, newMaximum, value);
        }

        public static IntermediateSegment
            <
                TMinimum, 
                TMaximum, 
                TNewMaximum, 
                Range
                    <
                        TMinimum, 
                        TMaximum, 
                        TNumeric, 
                        TValue
                    >
                    .StartingSegment, 
                TNumeric,
                TValue
            > 
            FollowedBy<TMinimum, TMaximum, TNewMaximum, TNumeric, TValue>(
                this Range<TMinimum, TMaximum, TNumeric, TValue>.StartingSegment segment, 
                TNewMaximum newMaximum, 
                TValue value)
            where TMinimum : TNumeric
            where TMaximum : TNumeric, IGreaterThan<TMinimum>
            where TNewMaximum : TNumeric, IGreaterThan<TMaximum>, IGreaterThan<TMinimum>
        {
            return new IntermediateSegment<TMinimum, TMaximum, TNewMaximum, Range<TMinimum, TMaximum, TNumeric, TValue>.StartingSegment, TNumeric, TValue>(
                segment,
                newMaximum,
                value);
        }
    }
}
