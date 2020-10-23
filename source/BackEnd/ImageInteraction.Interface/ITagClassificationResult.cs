namespace ImageInteraction.Interface
{
    public interface ITagClassificationResult
    {
        string TagName { get; }
        double Probability { get; }
    }
}
