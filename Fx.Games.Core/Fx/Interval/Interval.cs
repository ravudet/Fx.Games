namespace Fx.Interval
{
    using Fx.Numerics;

    public abstract class Interval<TMinimum, TMaximum, TNumeric, TValue> where TMaximum : TNumeric, IGreaterThan<TMinimum> where TMinimum : TNumeric
    {
        public sealed class Interval : Interval<TMinimum, TMaximum, TNumeric, TValue>
        {
        }
    }
}
