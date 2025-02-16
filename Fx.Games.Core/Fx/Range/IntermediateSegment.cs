using Microsoft.Win32.SafeHandles;
using System.Net.NetworkInformation;
using System.Threading;
using System.Xml.Linq;

namespace Fx.Range
{
    using Fx.Numerics;
    using Stash;

    //// TODO add the lessthan stuff //// TODO i don't remember why i wrote this, but maybe using ilessthan will let you nest stuff under the `range` abstract class?

    public sealed class IntermediateSegment<TMinimum, TIntermediate, TMaximum, TPreviousRange, TNumeric, TValue> : Range<TMinimum, TMaximum, TNumeric, TValue> where TPreviousRange : Range<TMinimum, TIntermediate, TNumeric, TValue> where TIntermediate : TNumeric, IGreaterThan<TMinimum> where TMaximum : TNumeric, IGreaterThan<TIntermediate>, IGreaterThan<TMinimum> where TMinimum : TNumeric
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

        protected internal override TResult Dispatch<TResult, TContext>(RangeVisitor<TMinimum, TMaximum, TValue, TNumeric, TResult, TContext> visitor, TContext context)
        {
            return visitor.Accept(this, context);
        }
    }

    public static class Range
    {
        public static Range<TMinimum, TMaximum, Natural, TValue>.StartingSegment Instance<TMinimum, TMaximum, TValue>(TMinimum minimum, TMaximum maximum, TValue value) where TMaximum : Natural, IGreaterThan<TMinimum> where TMinimum : Natural
        {
            return new Range<TMinimum, TMaximum, Natural, TValue>.StartingSegment(minimum, maximum, value);
        }
    }

    public static class Driver
    {
        public static void DoWork()
        {
            //// TODO try an operation that replaces the "maximum" of some segment by a different value (e.g. replace `four` with `three`)
            var range = Range
                .Instance(Naturals._1, Naturals._3, "first")
                .FollowedBy(Naturals._4, "second")
                .FollowedBy(Naturals._7, "third")
                .FollowedBy(Naturals._8, "fourth");

            //// TODO would it be nice to have something like Range.Start(_1).FollowedBy(-3, "first").FollowedBy(_4, "second")...?
        }
    }
}
