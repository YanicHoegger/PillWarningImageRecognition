using ImageInteraction.Interface;

namespace ImageInteraction.Classification
{
    public class TagClassificationResult : ITagClassificationResult
    {
        public TagClassificationResult(string tagName, double probability)
        {
            TagName = tagName;
            Probability = probability;
        }

        public string TagName { get; }
        public double Probability { get; }
    }
}
