namespace Fx.Numerics
{
    public static class NaturalExtensions
    {
        public static uint ToClr(this Natural natural)
        {
            return uint.Parse(natural.GetType().Name.Substring(1));
        }

        public static Natural Minus<TLeft, TRight>(this TLeft left, TRight right) where TLeft : Natural, IGreaterThan<TRight> where TRight : Natural
        {
            var difference = left.ToClr() - right.ToClr();

            return Natural.Create(difference);
        }
    }
}
