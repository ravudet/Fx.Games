namespace Fx.Range
{
    using Fx.Numerics;

    public abstract class Range<TMinimum, TMaximum, TValue> : IRange<TMinimum, TMaximum, TValue> where TMaximum : IGreaterThan<TMinimum>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Normally in the discriminated union pattern, this would be <see langword="private"/> and the members of the union would be nested, derived classes. However, because <see cref="IntermediateSegment{TMinimum, TIntermediate, TMaximum, TPreviousRange, TValue}"/> needs to further constrain <typeparamref name="TMaximum"/> to be greater than the intermediate, and c# does not allow constraints to be declared once the type parameter is defined, we have two options:
        /// 1. Nest <see cref="IntermediateSegment{TMinimum, TIntermediate, TMaximum, TPreviousRange, TValue}"/> under <see cref="Range{TMinimum, TMaximum, TValue}"/>, but have additional type parameters on <see cref="IntermediateSegment{TMinimum, TIntermediate, TMaximum, TPreviousRange, TValue}"/> (e.g. `TMaximum2` and `TMinimum2`). This would require callers to provide <typeparamref name="TMaximum"/> and <typeparamref name="TMinimum"/> for <see cref="Range{TMinimum, TMaximum, TValue}"/> when they are ultimately thrown away. This results in unnecessarily verbose code.
        /// 2. Allow <see cref="IntermediateSegment{TMinimum, TIntermediate, TMaximum, TPreviousRange, TValue}"/> to not be nested and rely on engineering rigor within this repository that new derived types with not be added to <see cref="Range{TMinimum, TMaximum, TValue}"/>.
        /// 
        /// We have chosen option 2 in order to reduce the barrier to use of these types overall.
        /// </remarks>
        internal protected Range()
        {
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
    }
}
