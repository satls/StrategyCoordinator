namespace StrategyCoordinator.Extensions.AspNetCoreExtension
{
    using Microsoft.Extensions.DependencyInjection;

    public class StrategyCoordinatorFactory<TIn, TOut, TContext> : StrategyCoordinator.Core.StrategyCoordinatorFactory<TIn, TOut, TContext> where TContext : StrategyCoordinator.Core.IContext<TIn, TOut>
    {
        private readonly System.IServiceProvider _serviceProvider;

        public StrategyCoordinatorFactory(StrategyCoordinator.Core.IContextFactory<TContext> contextFactory, System.IServiceProvider serviceProvider) : base(contextFactory)
        {
            this._serviceProvider = serviceProvider;
        }

        public override void UseAsync<TStrategy>()
        {
            var strategy = this._serviceProvider.GetService<TStrategy>();

            this.UseAsync(strategy);
        }
    }
}
