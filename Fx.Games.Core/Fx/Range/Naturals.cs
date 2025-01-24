namespace Fx.Range
{
    public interface ILessThan<T>
    {
    }

    public interface IGreaterThan<T>
    {
    }

    public abstract class Naturals
    {
        private Naturals()
        {
        }

        public abstract uint Value { get; } //// TODO you like that you convert back into a built-in type so quickly? should you abstract this somehow?

        public sealed class Zero : Naturals, ILessThanOne //// TODO rename these to _0
        {
            public override uint Value => 0;
        }

        private interface IGreaterThanZero : IGreaterThan<Zero>
        {
        }

        public sealed class One : Naturals, IGreaterThanZero
        {
            public override uint Value => 1;
        }

        private interface IGreaterThanOne : IGreaterThan<One>, IGreaterThanZero
        {
        }

        private interface ILessThanOne : ILessThan<One>
        {
        }

        public sealed class Two : Naturals, IGreaterThanOne
        {
            public override uint Value => 2;
        }

        private interface IGreaterThanTwo : IGreaterThan<Two>, IGreaterThanOne
        {
        }

        private interface ILessThanTwo : ILessThanOne, ILessThan<Two>
        {
        }

        public sealed class Three : Naturals, IGreaterThanTwo
        {
            public override uint Value => 3;
        }

        private interface IGreaterThanThree : IGreaterThan<Three>, IGreaterThanTwo
        {
        }

        public sealed class Four : Naturals, IGreaterThanThree
        {
            public override uint Value => 4;
        }

        private interface IGreaterThanFour : IGreaterThan<Four>, IGreaterThanThree
        {
        }

        public sealed class Five : Naturals, IGreaterThanFour
        {
            public override uint Value => 5;
        }

        private interface IGreaterThanFive : IGreaterThan<Five>, IGreaterThanFour
        {
        }

        public sealed class Six : Naturals, IGreaterThanFive
        {
            public override uint Value => 6;
        }

        private interface IGreaterThanSix : IGreaterThan<Six>, IGreaterThanFive
        {
        }

        public sealed class Seven : Naturals, IGreaterThanSix
        {
            public override uint Value => 7;
        }

        private interface IGreaterThanSeven : IGreaterThan<Seven>, IGreaterThanSix
        {
        }

        public sealed class Eight : Naturals, IGreaterThanSeven
        {
            public override uint Value => 8;
        }
    }
}
