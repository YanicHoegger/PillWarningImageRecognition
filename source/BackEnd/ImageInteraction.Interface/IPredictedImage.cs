namespace ImageInteraction.Interface
{
    public interface IPredictedImage : IImageClassificationResult
    {
        byte[] Image { get; }
    }
}
