namespace Fx.Distribution
{
    using Fx.Games.Game;

    public sealed class WeightedDistribution<TValue> : IDistribution<TValue, WeightedDistribution<TValue>>
    {
        private readonly IDistribution<int> uniformDistribution;

        public WeightedDistribution(IDistribution<int> uniformDistribution, PortionV2<TValue> portions)
        {
            //// TODO you toyed around with `sample` taking in a `idistribution` parameter so that distributions could be composed; in this constructor, you are composing a weighted distribution with a uniform distribution, but you've recognized in the comment below that allowed a `idistribution` in the constructor means that any caller of `weighteddistribution` may be properly assuming a weighted distribution, but they are really getting some composition (particularly if they take a look at the `portions` property); maybe what makes the most sense is `idistribution` having a `idistribution compose(idistribution)` method for the compositions, and `weighteddistribution` could take a `uniformdistribution` in as a parameter

            this.uniformDistribution = uniformDistribution; //// TODO for this class to really honor the weights, this needs to be a uniform distribution, but requiring specifically a uniform distribution removes composability; what do you want to do here? should there be a `compose` method that takes a new distribution or something?
            this.Portions = portions;
        }

        public PortionV2<TValue> Portions { get; }

        public TValue Sample(out WeightedDistribution<TValue>? remainder)
        {
            //// TODO i think in your old cod,e you want *this*.weights[chosenIndex] on the right hand side of the operations; for the "current node" case, you set weights[currentIndex] to 0, so in the higher nodes in the tree, you will subtract 0 if the leaf node was the current index
            throw new System.NotImplementedException();
        }

        public TValue Sample()
        {
            return this.Sample(out _);
        }
    }
}
