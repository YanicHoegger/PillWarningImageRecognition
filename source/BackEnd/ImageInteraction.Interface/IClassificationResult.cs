namespace ImageInteraction.Interface
{
    public interface IClassificationResult
    {
        string TagName { get; }
        double Probability { get; }
    }
}
