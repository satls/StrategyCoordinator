namespace StrategyCoordinator.Core
{
    public interface IAsyncProcessStrategy<TContext>
    {
        System.Threading.Tasks.Task ProcessAsync(TContext context, IInvokeable next);
    }
}
