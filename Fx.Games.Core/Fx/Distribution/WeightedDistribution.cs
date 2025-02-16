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

    public static class SegmentV2Extensions
    {
        public static System.Collections.Generic.IEnumerable<(double Weight, TValue Value)> ToWeights<TValue>(this SegmentV2<TValue, Natural> segment)
        {
            SegmentV2<TValue, Natural>? currentSegment = segment;

            var range = segment.GlobalMaximum.ToClr() - segment.GlobalMinimum.ToClr();
            while (currentSegment != null)
            {
                yield return
                    (
                        ((double)(currentSegment.Maximum.ToClr()) - currentSegment.Minimum.ToClr()) / range,
                        currentSegment.Value
                    );

                currentSegment = currentSegment.NextSegment;
            }
        }
    }

    public sealed class SegmentV2<TValue, TNatural>
    {
        public static SegmentV2<TValue, TNatural> Create<TMinimum, TMaximum>(Range<TMinimum, TMaximum, TNatural, TValue> segment)
            where TMaximum : TNatural, IGreaterThan<TMinimum>
            where TMinimum : TNatural
        {
            return CreateVisitor<TMinimum, TMaximum>.Instance.Visit(segment, default).Segment;
        }

        private sealed class CreateVisitor<TMinimum, TMaximum> : RangeVisitor<TMinimum, TMaximum, TValue, (SegmentV2<TValue, TNatural> Segment, TNatural NestedMaximum, TNatural GlobalMinimum), TNatural?, TNatural>
            where TMaximum : TNatural, IGreaterThan<TMinimum>
            where TMinimum : TNatural
        {
            private CreateVisitor()
            {
            }

            public static CreateVisitor<TMinimum, TMaximum> Instance { get; } = new CreateVisitor<TMinimum, TMaximum>();

            protected internal override (SegmentV2<TValue, TNatural> Segment, TNatural NestedMaximum, TNatural GlobalMinimum) Accept<TIntermediate, TMaximum2, TPreviousRange2>(IntermediateSegment<TMinimum, TIntermediate, TMaximum2, TPreviousRange2, TNatural, TValue> node, TNatural? context)
            {
                var globalMaximum = context ?? node.Maximmum;

                var nested = CreateVisitor<TMinimum, TIntermediate>.Instance.Visit(node.PreviousRange, globalMaximum);

                return (new SegmentV2<TValue, TNatural>(nested.GlobalMinimum, globalMaximum, nested.NestedMaximum, node.Maximmum, node.Value, nested.Segment), node.Maximmum, nested.GlobalMinimum);
            }

            protected internal override (SegmentV2<TValue, TNatural> Segment, TNatural NestedMaximum, TNatural GlobalMinimum) Accept(Range<TMinimum, TMaximum, TNatural, TValue>.StartingSegment node, TNatural? context)
            {
                return (new SegmentV2<TValue, TNatural>(node.Minimum, context!, node.Minimum, node.Maximum, node.Value, null), node.Maximum, node.Minimum);
            }
        }

        private SegmentV2(TNatural globalMinimum, TNatural globalMaximum, TNatural minimum, TNatural maximum, TValue value, SegmentV2<TValue, TNatural>? nextSegment)
        {
            GlobalMinimum = globalMinimum;
            GlobalMaximum = globalMaximum;
            Minimum = minimum;
            Maximum = maximum;
            Value = value;
            NextSegment = nextSegment;
        }

        public TNatural GlobalMinimum { get; }

        public TNatural GlobalMaximum { get; }

        public TNatural Minimum { get; }

        public TNatural Maximum { get; }

        public TValue Value { get; }

        public SegmentV2<TValue, TNatural>? NextSegment { get; }
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
