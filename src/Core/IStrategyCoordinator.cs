namespace StrategyCoordinator.Core
{
    public interface IStrategyCoordinator<TIn, TOut>
    {
        System.Threading.Tasks.Task<TOut> ProcessAsync(TIn input);
    }
}
