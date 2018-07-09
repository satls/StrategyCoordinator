namespace StrategyCoordinator.Core
{
    public class StrategyCoordinator<TIn, TOut, TContext> : IStrategyCoordinator<TIn, TOut> where TContext : IContext<TIn, TOut>
    {
        protected readonly System.Collections.Generic.IEnumerable<System.Func<TContext, IInvokeable, System.Threading.Tasks.Task>> _functions;

        protected readonly IContextFactory<TContext> _contextFactory;

        public StrategyCoordinator(System.Collections.Generic.IEnumerable<System.Func<TContext, IInvokeable, System.Threading.Tasks.Task>> functions, IContextFactory<TContext> contextFactory)
        {
            if (functions == null)
            {
                throw new System.ArgumentNullException("Must have a non null value for functions.");
            }

            if (contextFactory == null)
            {
                throw new System.ArgumentNullException("Must have a non null value for contextFactory.");
            }

            this._functions = functions;
            this._contextFactory = contextFactory;
        }

        public async System.Threading.Tasks.Task<TOut> ProcessAsync(TIn input)
        {
            TContext applicationContext = this._contextFactory.BuildContext();

            applicationContext.Input = input;

            IInvokeable applicationManager = new StrategyExecutor<TContext>(this._functions, applicationContext);

            await applicationManager.InvokeAsync();

            return applicationContext.Result;
        }
    }
}
