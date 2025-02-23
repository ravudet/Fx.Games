namespace Fx.Interval
{
    using System;

    using Fx.Numerics;

    public static class Interval
    {
        public static Interval<TMinimum, TMaximum, TNumeric, TValue>.Mesh Mesh<TMinimum, TMaximum, TNumeric, TValue>(TMinimum minimum, TMaximum maximum, TValue value, Type<TNumeric> numeric)
            where TMaximum : TNumeric, IGreaterThan<TMinimum>
            where TMinimum : TNumeric
        {
#pragma warning disable anidTODO // Type or member is obsolete
            return new Interval<TMinimum, TMaximum, TNumeric, TValue>.Mesh(minimum, maximum, value);
#pragma warning restore anidTODO // Type or member is obsolete
        }

        public static Interval<TMinimum, TIntermediate, TNumeric, TValue>.Partition<TIntermediate, TMaximum, TSubInterval> Partition<TMinimum, TIntermediate, TNumeric, TValue, TMaximum, TSubInterval>(TSubInterval subInterval, TMaximum maximum, TValue value)
            where TMinimum : TNumeric
            where TIntermediate : TNumeric, IGreaterThan<TMinimum>
            where TMaximum : TNumeric, IGreaterThan<TIntermediate>, IGreaterThan<TMinimum>
            where TSubInterval : Interval<TMinimum, TIntermediate, TNumeric, TValue>
        {
#pragma warning disable anotheridTODO // Type or member is obsolete
            return new Interval<TMinimum, TIntermediate, TNumeric, TValue>.Partition<TIntermediate, TMaximum, TSubInterval>(subInterval, maximum, value);
#pragma warning restore anotheridTODO // Type or member is obsolete
        }
    }
}
