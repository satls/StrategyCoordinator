namespace StrategyCoordinator.Core
{
    public interface IContextFactory<TContext>
    {
        TContext BuildContext();
    }
}
