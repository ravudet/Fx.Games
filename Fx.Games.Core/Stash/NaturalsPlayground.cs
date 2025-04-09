namespace Stash
{
    using System;

    using Fx;
    using Fx.Interval;
    using Fx.Numerics;
    using Fx.Range;

    public static class NaturalsPlayground
    {
        public static void DoWork()
        {
            var genericType = typeof(Range<,,,>.StartingSegment);
            var typeArguments = new[] { typeof(Natural._4), typeof(Natural._1), typeof(string) };
            var concreteType = genericType.MakeGenericType(typeArguments);
            //// TODO this throws because _1 doens't implement igreaterthan<_4>, which is a good thing; you need to start seeing how this range stuff can be leveraged by a game before going too much further cleaning up code; battleship or 2048 are good test beds
            var instance = Activator.CreateInstance(concreteType, Naturals._4, Naturals._1, string.Empty);
        }

        public static void DoWork(Natural natural)
        {
            var value =
                natural
                    .Visit(
                        Fx.Range.Range
                            .Instance(Naturals._0, Naturals._3, (Natural value, Nothing @void) => value.ToClr() * 1)
                            .FollowedBy(Naturals._8, (value, @void) => value.ToClr() * 2),
                        new Nothing());

            var otherValue = natural
                .Switch(
                    Fx.Interval.Interval.Partition<Natural._0, Natural._3, Natural, Func<Natural, Nothing, uint>, Natural._8, Fx.Interval.Interval<Natural._0, Natural._3, Natural, Func<Natural, Nothing, uint>>.Mesh>(
                        Fx.Interval.Interval.Mesh(
                            Naturals._0,
                            Naturals._3,
                            (Natural value, Nothing nothing) => value.ToClr() * 1,
                            Of.Type<Natural>()),
                        Naturals._8,
                        (value, nothing) => value.ToClr() * 2),
                    new Nothing());


        }

        public static TResult Switch<TNode, TResult, TContext, TMinimum, TMaximum, TNumeric>(
            this TNode node,
            Fx.Interval.Interval<TMinimum, TMaximum, TNumeric, Func<TNode, TContext, TResult>> range,
            TContext context)
            where TMaximum : TNumeric, IGreaterThan<TMinimum>
            where TMinimum : TNumeric
        {
            //// TODO you need to be able to say something like `where TNode : IGreaterThanable` for this to really make sense, and `IGreaterThan` needs to be coupled to that in some way to ensure that the `where TMaximum` constraint actually means something

            return SwitchVisitor<TNode, TResult, TContext, TMinimum, TMaximum, TNumeric>.Instance.Visit(range, (node, context));
        }

        private sealed class SwitchVisitor<TNode, TResult, TContext, TMinimum, TMaximum, TNumeric> :
            Fx.Interval.Interval<TMinimum, TMaximum, TNumeric, Func<TNode, TContext, TResult>>.IntervalVisitor<TResult, (TNode Node, TContext Context)>
            where TMaximum : TNumeric, IGreaterThan<TMinimum>
            where TMinimum : TNumeric
        {
            private SwitchVisitor()
            {
            }

            public static SwitchVisitor<TNode, TResult, TContext, TMinimum, TMaximum, TNumeric> Instance { get; } = new SwitchVisitor<TNode, TResult, TContext, TMinimum, TMaximum, TNumeric>();

            protected internal override TResult Accept(Interval<TMinimum, TMaximum, TNumeric, Func<TNode, TContext, TResult>>.Mesh node, (TNode Node, TContext Context) context)
            {
                if (context.Node is IGreaterThan<TMinimum>)
                {
                    throw new Exception("TODO context.node is not in the specified range");
                }

                return node.Value(context.Node, context.Context);
            }

            protected internal override TResult Accept<TMaximum2, TNewMaximum, TSubInterval2>(Interval<TMinimum, TMaximum2, TNumeric, Func<TNode, TContext, TResult>>.Partition<TNewMaximum, TSubInterval2> node, (TNode Node, TContext Context) context)
            {
                if (context.Node is IGreaterThan<TMaximum2>)
                {
                    return node.Value(context.Node, context.Context);
                }
                else
                {
                    return SwitchVisitor<TNode, TResult, TContext, TMinimum, TMaximum2, TNumeric>.Instance.Visit(node.SubInterval, context);
                }
            }
        }

        public static TResult Visit<TNode, TResult, TContext, TMinimum, TMaximum, TNumeric>(
            this TNode node,
            Range<TMinimum, TMaximum, TNumeric, Func<TNode, TContext, TResult>> range,
            TContext context)
            where TMaximum : TNumeric, IGreaterThan<TMinimum>
            where TMinimum : TNumeric
        {
            //// TODO you need to be able to say something like `where TNode : IGreaterThanable` for this to really make sense, and `IGreaterThan` needs to be coupled to that in some way to ensure that the `where TMaximum` constraint actually means something

            return VisitVisitor<TNode, TResult, TContext, TMinimum, TMaximum, TNumeric>.Instance.Visit(range, (node, context));
        }

        private sealed class VisitVisitor<TNode, TResult, TContext, TMinimum, TMaximum, TNumeric>
            : RangeVisitor<TMinimum, TMaximum, Func<TNode, TContext, TResult>, TNumeric, TResult, (TNode Node, TContext Context)>
            where TMaximum : TNumeric, IGreaterThan<TMinimum>
            where TMinimum : TNumeric
        {
            private VisitVisitor()
            {
            }

            public static VisitVisitor<TNode, TResult, TContext, TMinimum, TMaximum, TNumeric> Instance { get; } =
                new VisitVisitor<TNode, TResult, TContext, TMinimum, TMaximum, TNumeric>();

            protected internal override TResult Accept<TIntermediate, TMaximum2, TPreviousRange2>(IntermediateSegment<TMinimum, TIntermediate, TMaximum2, TPreviousRange2, TNumeric, Func<TNode, TContext, TResult>> node, (TNode Node, TContext Context) context)
            {
                if (context.Node is IGreaterThan<TIntermediate>)
                {
                    return node.Value(context.Node, context.Context);
                }
                else
                {
                    return VisitVisitor<TNode, TResult, TContext, TMinimum, TIntermediate, TNumeric>.Instance.Visit(node.PreviousRange, context);
                }
            }

            protected internal override TResult Accept(Range<TMinimum, TMaximum, TNumeric, Func<TNode, TContext, TResult>>.StartingSegment node, (TNode Node, TContext Context) context)
            {
                if (node is IGreaterThan<TMinimum>)//// TODO || TMinimum2 : IGreaterThan<TNode>)
                {
                    throw new Exception("TODO node is not in the specified range");
                }

                return node.Value(context.Node, context.Context);
            }
        }
    }
}
