namespace StrategyCoordinator.Extensions.AutofacExtension
{
    using Autofac;

    public class StrategyCoordinatorFactory<TIn, TOut, TContext> : StrategyCoordinator.Core.StrategyCoordinatorFactory<TIn, TOut, TContext> where TContext : StrategyCoordinator.Core.IContext<TIn, TOut>
    {
        private readonly Autofac.IContainer _container;

        public StrategyCoordinatorFactory(StrategyCoordinator.Core.IContextFactory<TContext> contextFactory, IContainer container) : base(contextFactory)
        {
            this._container = container;
        }

        public override void UseAsync<TStrategy>()
        {
            using (var scope = this._container.BeginLifetimeScope())
            {
                TStrategy strategy = scope.Resolve<TStrategy>();

                this.UseAsync(strategy);
            }
        }
    }
}
