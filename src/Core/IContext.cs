namespace StrategyCoordinator.Core
{
    public interface IContext<TIn, TOut>
    {
        TIn Input { get; set; }

        TOut Result { get; set; }
    }
}
