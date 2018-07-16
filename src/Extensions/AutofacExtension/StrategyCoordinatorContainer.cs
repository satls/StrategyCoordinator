namespace StrategyCoordinator.Extensions.AutofacExtension
{
    using Autofac;

    public static class StrategyCoordinatorContainer
    {
        private static IContainer Container;

        public static void UseContainer(IContainer container)
        {
            Container = container;
        }

        public static StrategyCoordinatorFactory<TIn, TOut, TContext> ResolveFactory<TIn, TOut, TContext>() where TContext : StrategyCoordinator.Core.IContext<TIn, TOut>
        {
            if(Container == null){
                throw new System.ArgumentException("No DI container has been set. Make sure UseContainer has been called with the desired Autofac.IContainer to use for dependency resolution.");
            }

            using (var scope = Container.BeginLifetimeScope())
            {
                StrategyCoordinator.Core.IContextFactory<TContext> contextFactory = scope.Resolve<StrategyCoordinator.Core.IContextFactory<TContext>>();

                return new StrategyCoordinatorFactory<TIn, TOut, TContext>(contextFactory, Container);
            }
        }
    }
}
