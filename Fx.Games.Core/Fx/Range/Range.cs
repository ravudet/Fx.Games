namespace Fx.Range
{
    using Fx.Numerics;

    public abstract class Range<TMinimum, TMaximum, TValue, TNatural> : IRange<TMinimum, TMaximum, TValue> where TMaximum : TNatural, IGreaterThan<TMinimum> where TMinimum : TNatural
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

        protected internal abstract TResult Dispatch<TResult, TContext>(RangeVisitor<TMinimum, TMaximum, TValue, TResult, TContext, TNatural> visitor, TContext context); //// TODO this should only be `protected`, but you're trying to see if it helps you implement the visitor and allow constraints on naturals *outside* of the `range` type

        public sealed class StartingSegment : Range<TMinimum, TMaximum, TValue, TNatural>, IRange<TMinimum, TMaximum, TValue>
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

            protected internal override TResult Dispatch<TResult, TContext>(RangeVisitor<TMinimum, TMaximum, TValue, TResult, TContext, TNatural> visitor, TContext context)
            {
                return visitor.Accept(this, context);
            }
        }
    }

    public abstract class RangeVisitor<TMinimum, TMaximum, TValue, TResult, TContext, TNatural> //// TODO does introducing tnumeric actually help you remove the naturals type constraint? why do you even need the additional type parameters on accept
        where TMaximum : TNatural, IGreaterThan<TMinimum>
        where TMinimum : TNatural
    {
        public TResult Visit(Range<TMinimum, TMaximum, TValue, TNatural> node, TContext context)
        {
            return node.Dispatch(this, context);
        }

        protected internal abstract TResult Accept(Range<TMinimum, TMaximum, TValue, TNatural>.StartingSegment node, TContext context);

        protected internal abstract TResult Accept(StartingSegment<TMinimum, TMaximum, TValue, TNatural> node, TContext context);
        protected internal abstract TResult Accept<TIntermediate, TMaximum2, TPreviousRange2>(IntermediateSegment<TMinimum, TIntermediate, TMaximum2, TPreviousRange2, TValue, TNatural> node, TContext context) where TPreviousRange2 : Range<TMinimum, TIntermediate, TValue, TNatural> where TIntermediate : TNatural, IGreaterThan<TMinimum> where TMaximum2 : TNatural, IGreaterThan<TIntermediate>, IGreaterThan<TMinimum>;
    }
}
