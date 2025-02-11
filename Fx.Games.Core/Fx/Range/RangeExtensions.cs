namespace Fx.Range
{
    using System.Collections.Generic;
    using System.Linq;

    using Fx;
    using Fx.Numerics;

    public static class RangeExtensions
    {
        public static IEnumerable<(double Weight, TValue Value)> ToWeights<TMinimum, TMaximum, TValue>(this Range<TMinimum, TMaximum, TValue> intermediateSegment) where TMaximum : Natural, IGreaterThan<TMinimum> where TMinimum : Natural
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
        private sealed class ToWeightsVisitor<TMinimum, TMaximum, TValue> : Range<TMinimum, TMaximum, TValue>.RangeVisitor<IEnumerable<(uint Range, TValue Value, uint Minimum, uint Intermediate)>, Nothing> where TMaximum : IGreaterThan<TMinimum>
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
}
