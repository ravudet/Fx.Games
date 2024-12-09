namespace Fx.Distribution
{
    public interface IDistribution<out TValue>
    {
        TValue Sample();
    }
}
