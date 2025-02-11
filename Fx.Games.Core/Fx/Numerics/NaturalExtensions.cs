namespace Fx.Numerics
{
    public static class NaturalExtensions
    {
        public static uint ToClr(this Natural natural)
        {
            return uint.Parse(natural.GetType().Name.Substring(1));
        }
    }
}
