namespace Fx.Distribution
{
    using Fx.Games.Game;

    public sealed class WeightedDistribution<TValue> : IDistribution<TValue, WeightedDistribution<TValue>>
    {
        private readonly IDistribution<int> uniformDistribution;

        public WeightedDistribution(IDistribution<int> uniformDistribution, PortionV2<TValue> portions)
        {
            this.uniformDistribution = uniformDistribution; //// TODO for this class to really honor the weights, this needs to be a uniform distribution, but requiring specifically a uniform distribution removes composability; what do you want to do here? should there be a `compose` method that takes a new distribution or something?
            this.Portions = portions;
        }

        public PortionV2<TValue> Portions { get; }

        public TValue Sample(out WeightedDistribution<TValue>? remainder)
        {
            throw new System.NotImplementedException();
        }

        public TValue Sample()
        {
            return this.Sample(out _);
        }
    }
}
