using ImageInteraction.Interface;

namespace ImageInteraction.Detection
{
    public class DetectionResult : IDetectionResult
    {
        public DetectionResult(IBoundingBox boundingBox, double probability)
        {
            BoundingBox = boundingBox;
            Probability = probability;
        }

        public IBoundingBox BoundingBox { get; }
        public double Probability { get; }
    }
}
