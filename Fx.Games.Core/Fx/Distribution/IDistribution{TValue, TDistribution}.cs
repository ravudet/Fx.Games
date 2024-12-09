namespace Fx.Distribution
{
    public interface IDistribution<out TValue, TDistribution> : IDistribution<TValue> where TDistribution : IDistribution<TValue, TDistribution>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="remainder">
        /// The current distribution, without the returned <typeparamref name="TValue"/>; the returned <typeparamref name="TValue"/> is selected without replacement in
        /// the resulting <typeparamref name="TDistribution"/>; this will be <see langword="null"/> if the returned <typeparamref name="TValue"/> is the last value in
        /// this distribution
        /// </param>
        /// <returns></returns>
        TValue Sample(out TDistribution? remainder);
    }
}
