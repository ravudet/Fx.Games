namespace Fx.Distribution
{
    public sealed class Univariate<TValue> : IDistribution<TValue>
    {
        public Univariate(TValue value)
        {
            Value = value;
        }

        public TValue Value { get; }

        public TValue Sample()
        {
            return this.Value;
        }
    }
}
