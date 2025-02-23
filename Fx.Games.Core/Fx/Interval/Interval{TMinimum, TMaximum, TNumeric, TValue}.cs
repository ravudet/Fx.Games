namespace Fx.Interval
{
    using Fx.Numerics;
    using Fx.Range;
    using System;

    public abstract class Interval<TMinimum, TMaximum, TNumeric, TValue> where TMaximum : TNumeric, IGreaterThan<TMinimum> where TMinimum : TNumeric
    {
        private Interval()
        {
        }

        protected internal abstract TResult Dispatch<TResult, TContext>(IntervalVisitor<TMinimum, TMaximum, TValue, TNumeric, TResult, TContext> visitor, TContext context);

        public sealed class Mesh : Interval<TMinimum, TMaximum, TNumeric, TValue>
        {
            [Obsolete("the message", DiagnosticId = "anidTODO")]
            internal Mesh(TMinimum minimum, TMaximum maximum, TValue value)
            {
            }

            protected internal override TResult Dispatch<TResult, TContext>(IntervalVisitor<TMinimum, TMaximum, TValue, TNumeric, TResult, TContext> visitor, TContext context)
            {
                return visitor.Accept(this, context);
            }
        }

        public sealed class Partition<TMaximum2, TSubInterval> : Interval<TMinimum, TMaximum2, TNumeric, TValue>
            where TMaximum2 : TNumeric, IGreaterThan<TMinimum>, IGreaterThan<TMaximum>
            where TSubInterval : Interval<TMinimum, TMaximum, TNumeric, TValue>
        {
            [Obsolete("the message", DiagnosticId = "anotheridTODO")]
            internal Partition(TSubInterval subInterval, TMaximum2 maximum, TValue value)
            {
            }

            protected internal override TResult Dispatch<TResult, TContext>(IntervalVisitor<TMinimum, TMaximum2, TValue, TNumeric, TResult, TContext> visitor, TContext context)
            {
                return visitor.Accept(this, context);
            }
        }
    }

    /// <summary>
    /// TODO make sure this visitor can be used to implement all of the place that rangevisitor is used right now
    /// TODO then move this visitor inside the `interval` class
    /// </summary>
    /// <typeparam name="TMinimum"></typeparam>
    /// <typeparam name="TMaximum"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="TNumeric"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TContext"></typeparam>
    public abstract class IntervalVisitor<TMinimum, TMaximum, TValue, TNumeric, TResult, TContext>
        where TMaximum : TNumeric, IGreaterThan<TMinimum>
        where TMinimum : TNumeric
    {
        public TResult Visit(Interval<TMinimum, TMaximum, TNumeric, TValue> node, TContext context)
        {
            return node.Dispatch(this, context);
        }

        protected internal abstract TResult Accept(Interval<TMinimum, TMaximum, TNumeric, TValue>.Mesh node, TContext context);

        protected internal abstract TResult Accept<TMinimum2, TMaximum2, TNumeric2, TValue2, TNewMaximum, TSubInterval2>(Interval<TMinimum2, TMaximum2, TNumeric2, TValue2>.Partition<TNewMaximum, TSubInterval2> node, TContext context)
            where TMinimum2 : TNumeric2
            where TMaximum2 : TNumeric2, IGreaterThan<TMinimum2>
            where TNewMaximum : TNumeric2, IGreaterThan<TMaximum2>, IGreaterThan<TMinimum2>
            where TSubInterval2 : Interval<TMinimum2, TMaximum2, TNumeric2, TValue2>;
    }
}
