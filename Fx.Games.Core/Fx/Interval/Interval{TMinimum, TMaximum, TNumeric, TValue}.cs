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

        public sealed class Partition<TIntermediate, TMaximum2, TSubInterval> : Interval<TMinimum, TMaximum2, TNumeric, TValue>
            where TIntermediate : TNumeric, IGreaterThan<TMinimum>
            where TMaximum2 : TNumeric, IGreaterThan<TMinimum>, IGreaterThan<TIntermediate>
            where TSubInterval : Interval<TMinimum, TIntermediate, TNumeric, TValue>
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

    public abstract class IntervalVisitor<TMinimum, TMaximum, TValue, TNumeric, TResult, TContext>
        where TMaximum : TNumeric, IGreaterThan<TMinimum>
        where TMinimum : TNumeric
    {
        public TResult Visit(Interval<TMinimum, TMaximum, TNumeric, TValue> node, TContext context)
        {
            return node.Dispatch(this, context);
        }

        protected internal abstract TResult Accept(Interval<TMinimum, TMaximum, TNumeric, TValue>.Mesh node, TContext context);

        protected internal abstract TResult Accept<TIntermediate, TMaximum2, TSubInterval>(Interval<TMinimum, TMaximum, TNumeric, TValue>.Partition<TIntermediate, TMaximum2, TSubInterval> node, TContext context) where TSubInterval : Interval<TMinimum, TIntermediate, TNumeric, TValue> where TIntermediate : TNumeric, IGreaterThan<TMinimum> where TMaximum2 : TNumeric, IGreaterThan<TIntermediate>, IGreaterThan<TMinimum>;
    }
}
