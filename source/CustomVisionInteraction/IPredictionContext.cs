namespace CustomVisionInteraction.Interface
{
    public interface IPredictionContext : IContext
    {
        string PublisherModelName { get; }
    }
}
