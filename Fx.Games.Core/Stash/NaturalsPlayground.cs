namespace Stash
{
    using System;

    using Fx;
    using Fx.Numerics;
    using Fx.Range;

    public static class NaturalsPlayground
    {
        public static void DoWork()
        {
            var genericType = typeof(StartingSegment<,,>);
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
        }

        public static TResult Visit<TNode, TResult, TContext, TMinimum, TMaximum>(
            this TNode node,
            Segment<TMinimum, TMaximum, Func<TNode, TContext, TResult>> range,
            TContext context)
            where TMaximum : IGreaterThan<TMinimum>
        {
            //// TODO you need to be able to say something like `where TNode : IGreaterThanable` for this to really make sense, and `IGreaterThan` needs to be coupled to that in some way to ensure that the `where TMaximum` constraint actually means something

            return VisitVisitor<TNode, TResult, TContext, TMinimum, TMaximum>.Instance.Visit(range, (node, context));
        }

        private sealed class VisitVisitor<TNode, TResult, TContext, TMinimum, TMaximum>
            : Segment<TMinimum, TMaximum, Func<TNode, TContext, TResult>>.Visitor<TResult, (TNode Node, TContext Context)>
            where TMaximum : IGreaterThan<TMinimum>
        {
            private VisitVisitor()
            {
            }

            public static VisitVisitor<TNode, TResult, TContext, TMinimum, TMaximum> Instance { get; } =
                new VisitVisitor<TNode, TResult, TContext, TMinimum, TMaximum>();

            protected internal override TResult Accept<TMinimum2, TMaximum2>(StartingSegment<TMinimum2, TMaximum2, Func<TNode, TContext, TResult>> node, (TNode Node, TContext Context) context)
            {
                if (node is IGreaterThan<TMinimum2>)//// TODO || TMinimum2 : IGreaterThan<TNode>)
                {
                    throw new Exception("TODO node is not in the specified range");
                }

                return node.Value(context.Node, context.Context);
            }

            protected internal override TResult Accept<TMinimum2, TIntermediate2, TMaximum2, TPreviousRange2>(IntermediateSegment<TMinimum2, TIntermediate2, TMaximum2, TPreviousRange2, Func<TNode, TContext, TResult>> node, (TNode Node, TContext Context) context)
            {
                if (context.Node is IGreaterThan<TIntermediate2>)
                {
                    return node.Value(context.Node, context.Context);
                }
                else
                {
                    return VisitVisitor<TNode, TResult, TContext, TMinimum2, TIntermediate2>.Instance.Visit(node.PreviousRange, context);
                }
            }
        }
    }
}
