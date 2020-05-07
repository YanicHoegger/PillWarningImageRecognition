namespace CustomVisionInteraction.Prediction
{
    public interface IPredictionContext : IContext
    {
        string PublisherModelName { get; }
    }
}
