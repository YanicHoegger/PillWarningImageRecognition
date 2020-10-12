using ImageInteraction.Interface;

namespace ImageInteraction.Classification
{
    public class ClassificationResult : IClassificationResult
    {
        public ClassificationResult(string tagName, double probability)
        {
            TagName = tagName;
            Probability = probability;
        }

        public string TagName { get; }
        public double Probability { get; }
    }
}
