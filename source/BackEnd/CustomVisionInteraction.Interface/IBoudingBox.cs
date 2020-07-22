namespace CustomVisionInteraction.Interface
{
    public interface IBoundingBox
    {
        double Left { get; }
        double Top { get; }
        double Width { get; }
        double Height { get; }
    }
}
