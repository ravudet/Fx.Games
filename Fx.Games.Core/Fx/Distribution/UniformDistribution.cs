namespace Fx.Distribution
{
    using System;

    public sealed class UniformDistribution : IDistribution<int>
    {
        private readonly Random random;

        public UniformDistribution(Random random)
        {
            this.random = random;
        }

        public int Sample()
        {
            return this.random.Next();
        }
    }
}
