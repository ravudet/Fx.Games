namespace Fx.Distribution
{
    using Fx.Games.Game;

    public static class UnivariateExtensions
    {
        public static PortionV2<TValue> ToPortion<TValue>(this Univariate<TValue> distribution)
        {
            return new PortionV2<TValue>.All(distribution.Value);
        }
    }
}
