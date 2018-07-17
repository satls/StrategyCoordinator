namespace StrategyCoordinator.Extensions.AspNetCoreExtension
{
    using Microsoft.Extensions.DependencyInjection;
    
    public static class StrategyCoordinatorContainer
    {
        private static System.IServiceProvider ServiceProvider;

        public static void UseServiceProvider(System.IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public static StrategyCoordinatorFactory<TIn, TOut, TContext> ResolveFactory<TIn, TOut, TContext>() where TContext : StrategyCoordinator.Core.IContext<TIn, TOut>
        {
            if (ServiceProvider == null)
            {
                throw new System.InvalidOperationException("No DI container has been set. Make sure UseContainer has been called with the desired Autofac.IContainer to use for dependency resolution.");
            }

            var contextFactory = ServiceProvider.GetService<StrategyCoordinator.Core.IContextFactory<TContext>>();

            return new StrategyCoordinatorFactory<TIn, TOut, TContext>(contextFactory, ServiceProvider);
        }
    }
}
