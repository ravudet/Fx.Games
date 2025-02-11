namespace Fx.Numerics
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// I prefer having concrete types for this instead of using static interfaces. If there are no instances, then type parameters will need to be specified in generics, and once 1 type parameter needs to be specified, all of them do, so the caller will end up losing out on a lot of type inference.
    /// </remarks>
    public abstract class Natural
    {
        private Natural()
        {
        }

        protected abstract TResult Dispatch<TResult>(Visitor<TResult> visitor);

        public abstract class Visitor<TResult>
        {
            public TResult Visit(Natural node)
            {
                return node.Dispatch(this);
            }

            protected internal abstract TResult Accept(Natural._0 node);
            protected internal abstract TResult Accept(Natural._1 node);
            protected internal abstract TResult Accept(Natural._2 node);
            protected internal abstract TResult Accept(Natural._3 node);
            protected internal abstract TResult Accept(Natural._4 node);
            protected internal abstract TResult Accept(Natural._5 node);
            protected internal abstract TResult Accept(Natural._6 node);
            protected internal abstract TResult Accept(Natural._7 node);
            protected internal abstract TResult Accept(Natural._8 node);
        }

        public sealed class _0 : Natural
        {
            private _0()
            {
            }

            public static _0 Instance { get; } = new _0();

            protected override TResult Dispatch<TResult>(Visitor<TResult> visitor)
            {
                return visitor.Accept(this);
            }
        }

        private interface IGreaterThan0 : IGreaterThan<_0>
        {
        }

        public sealed class _1 : Natural, IGreaterThan0
        {
            private _1()
            {
            }

            public static _1 Instance { get; } = new _1();

            protected override TResult Dispatch<TResult>(Visitor<TResult> visitor)
            {
                return visitor.Accept(this);
            }
        }

        private interface IGreaterThan1 : IGreaterThan<_1>, IGreaterThan0
        {
        }

        public sealed class _2 : Natural, IGreaterThan1
        {
            private _2()
            {
            }

            public static _2 Instance { get; } = new _2();

            protected override TResult Dispatch<TResult>(Visitor<TResult> visitor)
            {
                return visitor.Accept(this);
            }
        }

        private interface IGreaterThan2 : IGreaterThan<_2>, IGreaterThan1
        {
        }

        public sealed class _3 : Natural, IGreaterThan2
        {
            private _3()
            {
            }

            public static _3 Instance { get; } = new _3();

            protected override TResult Dispatch<TResult>(Visitor<TResult> visitor)
            {
                return visitor.Accept(this);
            }
        }

        private interface IGreaterThan3 : IGreaterThan<_3>, IGreaterThan2
        {
        }

        public sealed class _4 : Natural, IGreaterThan3
        {
            private _4()
            {
            }

            public static _4 Instance { get; } = new _4();

            protected override TResult Dispatch<TResult>(Visitor<TResult> visitor)
            {
                return visitor.Accept(this);
            }
        }

        private interface IGreaterThan4 : IGreaterThan<_4>, IGreaterThan3
        {
        }

        public sealed class _5 : Natural, IGreaterThan4
        {
            private _5()
            {
            }

            public static _5 Instance { get; } = new _5();

            protected override TResult Dispatch<TResult>(Visitor<TResult> visitor)
            {
                return visitor.Accept(this);
            }
        }

        private interface IGreaterThan5 : IGreaterThan<_5>, IGreaterThan4
        {
        }

        public sealed class _6 : Natural, IGreaterThan5
        {
            private _6()
            {
            }

            public static _6 Instance { get; } = new _6();

            protected override TResult Dispatch<TResult>(Visitor<TResult> visitor)
            {
                return visitor.Accept(this);
            }
        }

        private interface IGreaterThan6 : IGreaterThan<_6>, IGreaterThan5
        {
        }

        public sealed class _7 : Natural, IGreaterThan6
        {
            private _7()
            {
            }

            public static _7 Instance { get; } = new _7();

            protected override TResult Dispatch<TResult>(Visitor<TResult> visitor)
            {
                return visitor.Accept(this);
            }
        }

        private interface IGreaterThan8 : IGreaterThan<_7>, IGreaterThan6
        {
        }

        public sealed class _8 : Natural, IGreaterThan8
        {
            private _8()
            {
            }

            public static _8 Instance { get; } = new _8();

            protected override TResult Dispatch<TResult>(Visitor<TResult> visitor)
            {
                return visitor.Accept(this);
            }
        }
    }
}
