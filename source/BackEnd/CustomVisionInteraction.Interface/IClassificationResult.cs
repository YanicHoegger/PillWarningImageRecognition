namespace CustomVisionInteraction.Interface
{
    public interface IClassificationResult
    {
        string TagName { get; }
        double Probability { get; }
    }
}
