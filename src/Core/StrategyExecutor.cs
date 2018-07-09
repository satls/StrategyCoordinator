namespace StrategyCoordinator.Core
{
    class StrategyExecutor<TContext> : IInvokeable
    {
        private readonly System.Collections.Generic.Queue<System.Func<TContext, IInvokeable, System.Threading.Tasks.Task>> _functionQueue;

        private readonly TContext _applicationContext;

        public StrategyExecutor(System.Collections.Generic.IEnumerable<System.Func<TContext, IInvokeable, System.Threading.Tasks.Task>> functions, TContext context)
        {
            this._applicationContext = context;

            this._functionQueue = new System.Collections.Generic.Queue<System.Func<TContext, IInvokeable, System.Threading.Tasks.Task>>();

            foreach (var function in functions)
            {
                this._functionQueue.Enqueue(function);
            }
        }

        public async System.Threading.Tasks.Task InvokeAsync()
        {
            if (this._functionQueue.Count > 0)
            {
                var function = this._functionQueue.Dequeue();

                if (function == null)
                {
                    throw new System.ArgumentNullException($"A null value was dequeued from the function queue. Make sure all functions queued in the application are not null.");
                }

                await function.Invoke(this._applicationContext, this);
            }
        }
    }
}
