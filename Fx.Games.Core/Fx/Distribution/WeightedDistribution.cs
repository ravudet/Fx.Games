namespace Fx.Distribution
{
    using Fx.Games.Game;
    using Fx.Range;
    using System.Net.Mime;
    using System.Xml;
    using System.Xml.Serialization;

    using Fx.Numerics;
    using Stash;
    using static Fx.Games.Game.NewAttempt;
    using System.Numerics;
    using Fx.Interval;

    public static class SegmentV2Extensions
    {
        public static System.Collections.Generic.IEnumerable<(double Weight, TValue Value)> ToWeights<TValue, TNumeric>(this SegmentV2<TValue, TNumeric> segment)
            where TNumeric : ISubtractionOperators<TNumeric, TNumeric, TNumeric>, IDivisionOperators<TNumeric, TNumeric, double>  //// TODO why double?
        {
            SegmentV2<TValue, TNumeric>? currentSegment = segment;

            var range = segment.GlobalMaximum - segment.GlobalMinimum;
            while (currentSegment != null)
            {
                yield return
                    (
                        (currentSegment.Maximum - currentSegment.Minimum) / range,
                        currentSegment.Value
                    );

                currentSegment = currentSegment.NextSegment;
            }
        }

        public static System.Collections.Generic.IEnumerable<(double Weight, TValue Value)> ToWeights<TValue, TNumeric>(this SegmentV3<TValue, TNumeric> segment)
            where TNumeric : ISubtractionOperators<TNumeric, TNumeric, TNumeric>, IDivisionOperators<TNumeric, TNumeric, double>  //// TODO why double?
        {
            SegmentV3<TValue, TNumeric>? currentSegment = segment;

            var range = segment.GlobalMaximum - segment.GlobalMinimum;
            while (currentSegment != null)
            {
                yield return
                    (
                        (currentSegment.Maximum - currentSegment.Minimum) / range,
                        currentSegment.Value
                    );

                currentSegment = currentSegment.NextSegment;
            }
        }
    }

    public sealed class SegmentV3<TValue, TNumeric>
    {
        //// TODO only rational weights can be modeled with this; maybe you should have a `create` overload that just uses doubles directly and then makes the assertions about them
        
        public static SegmentV3<TValue, TNumeric> Create<TMinimum, TMaximum>(Interval<TMinimum, TMaximum, TNumeric, TValue> segment)
            where TMaximum : TNumeric, IGreaterThan<TMinimum>
            where TMinimum : TNumeric
        {
            //// TODO i don't think you should need the type parameters of `segment` to get to this create method
            return CreateVisitor<TMinimum, TMaximum>.Instance.Visit(segment, default).Segment;
        }

        private sealed class CreateVisitor<TMinimum, TMaximum> : Fx.Interval.Interval<TMinimum, TMaximum, TNumeric, TValue>.IntervalVisitor<(SegmentV3<TValue, TNumeric> Segment, TNumeric NestedMaximum, TNumeric GlobalMinimum), TNumeric?>
            where TMaximum : TNumeric, IGreaterThan<TMinimum>
            where TMinimum : TNumeric
        {
            private CreateVisitor()
            {
            }

            public static CreateVisitor<TMinimum, TMaximum> Instance { get; } = new CreateVisitor<TMinimum, TMaximum>();

            protected internal override (SegmentV3<TValue, TNumeric> Segment, TNumeric NestedMaximum, TNumeric GlobalMinimum) Accept(Interval<TMinimum, TMaximum, TNumeric, TValue>.Mesh node, TNumeric? context)
            {
                //// TODO can you remove the null forgiving on `context`?
                return (new SegmentV3<TValue, TNumeric>(node.Minimum, context!, node.Minimum, node.Maximum, node.Value, null), node.Maximum, node.Minimum);
            }

            protected internal override (SegmentV3<TValue, TNumeric> Segment, TNumeric NestedMaximum, TNumeric GlobalMinimum) Accept<TMaximum2, TNewMaximum, TSubInterval2>(Interval<TMinimum, TMaximum2, TNumeric, TValue>.Partition<TNewMaximum, TSubInterval2> node, TNumeric? context)
            {
                var globalMaximum = context ?? node.Maximum;

                var nested = CreateVisitor<TMinimum, TMaximum2>.Instance.Visit(node.SubInterval, globalMaximum);

                return (new SegmentV3<TValue, TNumeric>(nested.GlobalMinimum, globalMaximum, nested.NestedMaximum, node.Maximum, node.Value, nested.Segment), node.Maximum, nested.GlobalMinimum);
            }
        }

        private SegmentV3(TNumeric globalMinimum, TNumeric globalMaximum, TNumeric minimum, TNumeric maximum, TValue value, SegmentV3<TValue, TNumeric>? nextSegment)
        {
            GlobalMinimum = globalMinimum;
            GlobalMaximum = globalMaximum;
            Minimum = minimum;
            Maximum = maximum;
            Value = value;
            NextSegment = nextSegment;
        }

        public TNumeric GlobalMinimum { get; }

        public TNumeric GlobalMaximum { get; }

        public TNumeric Minimum { get; }

        public TNumeric Maximum { get; }

        public TValue Value { get; }

        public SegmentV3<TValue, TNumeric>? NextSegment { get; }
    }

    public sealed class SegmentV2<TValue, TNumeric>
    {
        //// TODO only rational weights can be modeled with this; maybe you should have a `create` overload that just uses doubles directly and then makes the assertions about them

        public static SegmentV2<TValue, TNumeric> Create<TMinimum, TMaximum>(Range<TMinimum, TMaximum, TNumeric, TValue> segment)
            where TMaximum : TNumeric, IGreaterThan<TMinimum>
            where TMinimum : TNumeric
        {
            //// TODO i don't think you should need the type parameters of `segment` to get to this create method
            return CreateVisitor<TMinimum, TMaximum>.Instance.Visit(segment, default).Segment;
        }

        private sealed class CreateVisitor<TMinimum, TMaximum> : RangeVisitor<TMinimum, TMaximum, TValue, TNumeric,  (SegmentV2<TValue, TNumeric> Segment, TNumeric NestedMaximum, TNumeric GlobalMinimum), TNumeric?>
            where TMaximum : TNumeric, IGreaterThan<TMinimum>
            where TMinimum : TNumeric
        {
            private CreateVisitor()
            {
            }

            public static CreateVisitor<TMinimum, TMaximum> Instance { get; } = new CreateVisitor<TMinimum, TMaximum>();

            protected internal override (SegmentV2<TValue, TNumeric> Segment, TNumeric NestedMaximum, TNumeric GlobalMinimum) Accept<TIntermediate, TMaximum2, TPreviousRange2>(IntermediateSegment<TMinimum, TIntermediate, TMaximum2, TPreviousRange2, TNumeric, TValue> node, TNumeric? context)
            {
                var globalMaximum = context ?? node.Maximum;

                var nested = CreateVisitor<TMinimum, TIntermediate>.Instance.Visit(node.PreviousRange, globalMaximum);

                return (new SegmentV2<TValue, TNumeric>(nested.GlobalMinimum, globalMaximum, nested.NestedMaximum, node.Maximum, node.Value, nested.Segment), node.Maximum, nested.GlobalMinimum);
            }

            protected internal override (SegmentV2<TValue, TNumeric> Segment, TNumeric NestedMaximum, TNumeric GlobalMinimum) Accept(Range<TMinimum, TMaximum, TNumeric, TValue>.StartingSegment node, TNumeric? context)
            {
                return (new SegmentV2<TValue, TNumeric>(node.Minimum, context!, node.Minimum, node.Maximum, node.Value, null), node.Maximum, node.Minimum);
            }
        }

        private SegmentV2(TNumeric globalMinimum, TNumeric globalMaximum, TNumeric minimum, TNumeric maximum, TValue value, SegmentV2<TValue, TNumeric>? nextSegment)
        {
            GlobalMinimum = globalMinimum;
            GlobalMaximum = globalMaximum;
            Minimum = minimum;
            Maximum = maximum;
            Value = value;
            NextSegment = nextSegment;
        }

        public TNumeric GlobalMinimum { get; }

        public TNumeric GlobalMaximum { get; }

        public TNumeric Minimum { get; }

        public TNumeric Maximum { get; }

        public TValue Value { get; }

        public SegmentV2<TValue, TNumeric>? NextSegment { get; }
    }

    public sealed class WeightedDistribution<TValue> : IDistribution<TValue, WeightedDistribution<TValue>>
    {
        private readonly IDistribution<int> uniformDistribution;

        public WeightedDistribution(IDistribution<int> uniformDistribution, SegmentV2<TValue, Natural> range)
        {
            this.uniformDistribution = uniformDistribution;
            Range = range;
        }

        public WeightedDistribution(IDistribution<int> uniformDistribution, PortionV2<TValue> portions)
        {
            //// TODO you toyed around with `sample` taking in a `idistribution` parameter so that distributions could be composed; in this constructor, you are composing a weighted distribution with a uniform distribution, but you've recognized in the comment below that allowed a `idistribution` in the constructor means that any caller of `weighteddistribution` may be properly assuming a weighted distribution, but they are really getting some composition (particularly if they take a look at the `portions` property); maybe what makes the most sense is `idistribution` having a `idistribution compose(idistribution)` method for the compositions, and `weighteddistribution` could take a `uniformdistribution` in as a parameter

            this.uniformDistribution = uniformDistribution; //// TODO for this class to really honor the weights, this needs to be a uniform distribution, but requiring specifically a uniform distribution removes composability; what do you want to do here? should there be a `compose` method that takes a new distribution or something?
            this.Portions = portions;
        }

        public PortionV2<TValue> Portions { get; }
        public SegmentV2<TValue, Natural> Range { get; }

        public TValue Sample(out WeightedDistribution<TValue>? remainder)
        {
            //// TODO i think in your old cod,e you want *this*.weights[chosenIndex] on the right hand side of the operations; for the "current node" case, you set weights[currentIndex] to 0, so in the higher nodes in the tree, you will subtract 0 if the leaf node was the current index
            
            //// TODO i think you need to redo `portionv2`; it's basically impossible to get portions of equal weight
            throw new System.NotImplementedException();
        }

        public TValue Sample()
        {
            return this.Sample(out _);
        }
    }
}
