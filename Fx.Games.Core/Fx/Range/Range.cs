namespace Fx.Range
{
    using Fx.Numerics;

    public abstract class Range<TMinimum, TMaximum, TValue> : IRange<TMinimum, TMaximum, TValue> where TMaximum : IGreaterThan<TMinimum>
    {
        internal protected Range()
        {
            //// TODO should be private, and the derived classes should be nested
        }

        public abstract TValue Value { get; }

        protected abstract TResult Dispatch<TResult, TContext>(RangeVisitor<TResult, TContext> visitor, TContext context);

        public abstract class RangeVisitor<TResult, TContext>
        {
            public TResult Visit(Range<TMinimum, TMaximum, TValue> node, TContext context)
            {
                return node.Dispatch(this, context);
            }

            protected internal abstract TResult Accept<TMinimum2, TMaximum2>(Range<TMinimum2, TMaximum2, TValue>.StartingSegment node, TContext context) where TMaximum2 : IGreaterThan<TMinimum2>; //// TODO you shouldn't have to specify `naturals` here, it should be somehow in the derived type

            protected internal abstract TResult Accept<TMinimum2, TMaximum2>(StartingSegment<TMinimum2, TMaximum2, TValue> node, TContext context) where TMinimum2 : Natural where TMaximum2 : Natural, IGreaterThan<TMinimum2>; //// TODO you shouldn't have to specify `naturals` here, it should be somehow in the derived type
            protected internal abstract TResult Accept<TMinimum2, TIntermediate2, TMaximum2, TPreviousRange2>(IntermediateSegment<TMinimum2, TIntermediate2, TMaximum2, TPreviousRange2, TValue> node, TContext context) where TPreviousRange2 : Range<TMinimum2, TIntermediate2, TValue>, IRange<TMinimum2, TIntermediate2, TValue> where TIntermediate2 : Natural, IGreaterThan<TMinimum2> where TMaximum2 : Natural, IGreaterThan<TIntermediate2>, IGreaterThan<TMinimum2> where TMinimum2 : Natural;
        }

        public sealed class StartingSegment : Range<TMinimum, TMaximum, TValue>, IRange<TMinimum, TMaximum, TValue>
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

            /*public IntermediateSegment<TMinimum, TMaximum, TNewMaximum, StartingSegment<TMinimum, TMaximum, TValue>, TValue> FollowedBy<TNewMaximum>(TNewMaximum newMaximum, TValue value) where TNewMaximum : Natural, IGreaterThan<TMaximum>, IGreaterThan<TMinimum>
            {
                return new IntermediateSegment<TMinimum, TMaximum, TNewMaximum, StartingSegment<TMinimum, TMaximum, TValue>, TValue>(
                    this,
                    newMaximum,
                    value);
            }*/

            protected override TResult Dispatch<TResult, TContext>(RangeVisitor<TResult, TContext> visitor, TContext context)
            {
                return visitor.Accept(this, context);
            }
        }

        public sealed class IntermediateSegment<TIntermediate, TPreviousRange> : Range<TMinimum, TMaximum, TValue>, IRange<TMinimum, TMaximum, TValue> where TPreviousRange : Range<TMinimum, TIntermediate, TValue>, IRange<TMinimum, TIntermediate, TValue> where TIntermediate : Natural, IGreaterThan<TMinimum> where TMaximum : Natural, IGreaterThan<TIntermediate>, IGreaterThan<TMinimum> where TMinimum : Natural
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

            protected override TResult Dispatch<TResult, TContext>(RangeVisitor<TResult, TContext> visitor, TContext context)
            {
                return visitor.Accept(this, context);
            }
        }
    }
}
