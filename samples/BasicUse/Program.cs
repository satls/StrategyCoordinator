namespace BasicUse
{
    class Program
    {
        static void Main(string[] args)
        {
            var contextFactory = new ContextFactory();

            var coordinatorBuilder = new StrategyCoordinator.Core.StrategyCoordinatorFactory<string, string, Context>(contextFactory);

            coordinatorBuilder.UseAsync(new ReverseInputStrategy());

            var coordinator = coordinatorBuilder.Build();

            var output = coordinator.ProcessAsync("Hello World!").Result;

            System.Console.WriteLine(output);
        }
    }

    public class Context : StrategyCoordinator.Core.IContext<string, string>
    {
        public string Input { get; set; }
        public string Result { get; set; }
    }

    public class ContextFactory : StrategyCoordinator.Core.IContextFactory<Context>
    {
        public Context BuildContext()
        {
            return new Context();
        }
    }

    public class ReverseInputStrategy: StrategyCoordinator.Core.IAsyncProcessStrategy<Context>
    {
        public async System.Threading.Tasks.Task ProcessAsync(Context context, StrategyCoordinator.Core.IInvokeable next)
        {
            System.Collections.Generic.IEnumerable<char> chars = System.Linq.Enumerable.Select(context.Input, x => x);

            System.Collections.Generic.IEnumerable<char> reverseChars = System.Linq.Enumerable.Reverse(chars);

            string reverse = string.Join(string.Empty, reverseChars); 

            context.Result = reverse;

            await next.InvokeAsync();
        }
    }
}
