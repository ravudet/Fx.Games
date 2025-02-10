using System;
using System.Net.Mime;
using System.Runtime.InteropServices;

namespace Fx.Range
{
    public static class NaturalsPlayground
    {
        public static void DoWork()
        {
            var genericType = typeof(StartingSegment<,,>);
            var typeArguments = new[] { typeof(Naturals.Types._4), typeof(Naturals.Types._1), typeof(string) };
            var concreteType = genericType.MakeGenericType(typeArguments);
            //// TODO this throws because _1 doens't implement igreaterthan<_4>, which is a good thing; you need to start seeing how this range stuff can be leveraged by a game before going too much further cleaning up code; battleship or 2048 are good test beds
            var instance = Activator.CreateInstance(concreteType, Naturals._4, Naturals._1, string.Empty);
        }

        public static void DoWork(Naturals natural)
        {
            var value = 
                natural
                    .Visit(
                        Range
                            .Instance(Naturals._0, Naturals._3, (Naturals value, Void @void) => value.ToClr() * 1)
                            .FollowedBy(Naturals._8, (Naturals value, Void @void) => value.ToClr() * 2),
                        new Void());
        }

        public static uint ToClr(this Naturals natural)
        {
            return uint.Parse(natural.GetType().Name.Substring(1));
        }

        public static TResult Visit<TNode, TResult, TContext, TMinimum, TMaximum>(
            this TNode node, 
            Segment<TMinimum, TMaximum, Func<TNode, TContext, TResult>> range,
            TContext context)
            where TMaximum : IGreaterThan<TMinimum>
        {
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

    public interface IGreaterThan<T>
    {
    }

    public abstract class Naturals //// TODO should this be plural? do you want the static properties to be in static class called naturals, and have the du be a class called natural?
    {
        private Naturals()
        {
            //// TODO i think you prefer having concrete instances instead of just types; if you just have types, you would need to specify type parameters more often, which will result in losing all type inference
        }

        //// public abstract uint Value { get; } //// TODO you like that you convert back into a built-in type so quickly? should you abstract this somehow? //// TODO use a visitor for this? //// TODO have a visitor, and then have a visit method that takes in a range where the values are the accept methods

        /*public TResult Visit<TResult, TContext>(Segment<Types._0, Types._8, Func<Naturals, TContext, TResult>> range, TContext context)
        {
            //// TODO is this method signature actually valuable? the `func`s still won't know exactly which natural they are being given, so they will still need to do something more

            return VisitVisitor<TResult, TContext, Naturals.Types._0, Naturals.Types._8>.Instance.Visit(range, (this, context));
        }*/

        private sealed class VisitVisitor<TResult, TContext, TMinimum, TMaximum>
            : Segment<TMinimum, TMaximum, Func<Naturals, TContext, TResult>>.Visitor<TResult, (Naturals Natural, TContext Context)>
            where TMaximum : IGreaterThan<TMinimum>
        {
            private VisitVisitor()
            {
            }

            public static VisitVisitor<TResult, TContext, TMinimum, TMaximum> Instance { get; } =
                new VisitVisitor<TResult, TContext, TMinimum, TMaximum>();

            protected internal override TResult Accept<TMinimum2, TMaximum2>(
                StartingSegment<TMinimum2, TMaximum2, Func<Naturals, TContext, TResult>> node,
                (Naturals Natural, TContext Context) context)
            {
                if (context.Natural is IGreaterThan<TMinimum2> && !(context.Natural is IGreaterThan<TMaximum2>))
                {
                    //// TODO this if statement isn't actually inclusive of the whole range...

                    return node.Value(context.Natural, context.Context);
                }
                else
                {
                    throw new Exception("TODO this means that they gave us a natural that somehow doesn't fit into the range, this is a bug");
                }
            }

            protected internal override TResult Accept<TMinimum2, TIntermediate2, TMaximum2, TPreviousRange2>(
                IntermediateSegment<TMinimum2, TIntermediate2, TMaximum2, TPreviousRange2, Func<Naturals, TContext, TResult>> node,
                (Naturals Natural, TContext Context) context)
            {
                if (context.Natural is IGreaterThan<TIntermediate2>)
                {
                    return node.Value(context.Natural, context.Context);
                }
                else
                {
                    return VisitVisitor<TResult, TContext, TMinimum2, TIntermediate2>.Instance.Visit(node.PreviousRange, context);
                }
            }
        }

        public static Types._0 _0 { get; } = Types._0.Instance;
        public static Types._1 _1 { get; } = Types._1.Instance;
        public static Types._2 _2 { get; } = Types._2.Instance;
        public static Types._3 _3 { get; } = Types._3.Instance;
        public static Types._4 _4 { get; } = Types._4.Instance;
        public static Types._5 _5 { get; } = Types._5.Instance;
        public static Types._6 _6 { get; } = Types._6.Instance;
        public static Types._7 _7 { get; } = Types._7.Instance;
        public static Types._8 _8 { get; } = Types._8.Instance;

        public static class Types
        {
            public sealed class _0 : Naturals
            {
                private _0()
                {
                }

                public static _0 Instance { get; } = new _0();
            }

            private interface IGreaterThan0 : IGreaterThan<_0>
            {
            }

            public sealed class _1 : Naturals, IGreaterThan0
            {
                private _1()
                {
                }

                public static _1 Instance { get; } = new _1();
            }

            private interface IGreaterThan1 : IGreaterThan<_1>, IGreaterThan0
            {
            }

            public sealed class _2 : Naturals, IGreaterThan1
            {
                private _2()
                {
                }

                public static _2 Instance { get; } = new _2();
            }

            private interface IGreaterThan2 : IGreaterThan<_2>, IGreaterThan1
            {
            }

            public sealed class _3 : Naturals, IGreaterThan2
            {
                private _3()
                {
                }

                public static _3 Instance { get; } = new _3();
            }

            private interface IGreaterThan3 : IGreaterThan<_3>, IGreaterThan2
            {
            }

            public sealed class _4 : Naturals, IGreaterThan3
            {
                private _4()
                {
                }

                public static _4 Instance { get; } = new _4();
            }

            private interface IGreaterThan4 : IGreaterThan<_4>, IGreaterThan3
            {
            }

            public sealed class _5 : Naturals, IGreaterThan4
            {
                private _5()
                {
                }

                public static _5 Instance { get; } = new _5();
            }

            private interface IGreaterThan5 : IGreaterThan<_5>, IGreaterThan4
            {
            }

            public sealed class _6 : Naturals, IGreaterThan5
            {
                private _6()
                {
                }

                public static _6 Instance { get; } = new _6();
            }

            private interface IGreaterThan6 : IGreaterThan<_6>, IGreaterThan5
            {
            }

            public sealed class _7 : Naturals, IGreaterThan6
            {
                private _7()
                {
                }

                public static _7 Instance { get; } = new _7();
            }

            private interface IGreaterThan8 : IGreaterThan<_7>, IGreaterThan6
            {
            }

            public sealed class _8 : Naturals, IGreaterThan8
            {
                private _8()
                {
                }

                public static _8 Instance { get; } = new _8();
            }
        }
    }
}
