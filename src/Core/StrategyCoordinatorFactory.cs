namespace StrategyCoordinator.Core
{
    public class StrategyCoordinatorFactory<TIn, TOut, TContext> : IStrategyCoordinatorFactory<TIn, TOut, TContext> where TContext : IContext<TIn, TOut>
    {
        private readonly IContextFactory<TContext> _contextFactory;

        private readonly System.Collections.Generic.List<System.Func<TContext, IInvokeable, System.Threading.Tasks.Task>> _functions = new System.Collections.Generic.List<System.Func<TContext, IInvokeable, System.Threading.Tasks.Task>>();

        public StrategyCoordinatorFactory(IContextFactory<TContext> contextFactory)
        {
            if (contextFactory == null)
            {
                throw new System.ArgumentNullException("The context factory cannot be null.");
            }

            this._contextFactory = contextFactory;
        }

        public virtual void UseAsync(System.Func<TContext, IInvokeable, System.Threading.Tasks.Task> function)
        {
            if (function == null)
            {
                throw new System.ArgumentNullException("Tried to queue a null value for function.");
            }

            this._functions.Add(function);
        }

        public virtual void UseAsync(IAsyncProcessStrategy<TContext> strategy)
        {
            if (strategy == null)
            {
                throw new System.ArgumentNullException("Tried to queue a null value for strategy.");
            }

            this.UseAsync(strategy.ProcessAsync);
        }

        public virtual void UseAsync<TStrategy>() where TStrategy : IAsyncProcessStrategy<TContext>
        {
            var strategyType = typeof(TStrategy);

            IAsyncProcessStrategy<TContext> strategy = (TStrategy)System.Activator.CreateInstance(strategyType);

            this.UseAsync(strategy);
        }

        public virtual IStrategyCoordinator<TIn, TOut> Build()
        {
            return new StrategyCoordinator<TIn, TOut, TContext>(this._functions, this._contextFactory);
        }
    }
}
