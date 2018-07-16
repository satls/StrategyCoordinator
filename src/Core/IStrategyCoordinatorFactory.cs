namespace StrategyCoordinator.Core
{
    public interface IStrategyCoordinatorFactory<TIn, TOut, TContext> where TContext : IContext<TIn, TOut>
    {
        void UseAsync(System.Func<TContext, IInvokeable, System.Threading.Tasks.Task> function);

        void UseAsync(IAsyncProcessStrategy<TContext> strategy);

        IStrategyCoordinator<TIn, TOut> Build();
    }
}
