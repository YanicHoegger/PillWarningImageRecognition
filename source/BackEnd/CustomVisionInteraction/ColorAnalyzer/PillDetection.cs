using CustomVisionInteraction.Interface;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CustomVisionInteraction.ColorAnalyzer
{
    public class PillDetection : IPillDetection
    {
        private readonly IPillDetectionCommunication _pillDetectionCommunication;

        private const double MinimumPropabilityForPill = 0.9;

        public PillDetection(IPillDetectionCommunication pillDetectionCommunication)
        {
            _pillDetectionCommunication = pillDetectionCommunication;
        }

        public async Task<(bool hasDetection, BoundingBox boundingBox)> GetBestDetection(byte[] image)
        {
            var result = await _pillDetectionCommunication.DetectPill(new MemoryStream(image));

            var bestMatch = result.Predictions.OrderBy(x => x.Probability).LastOrDefault(x => x.Probability > MinimumPropabilityForPill);

            return (bestMatch != null, bestMatch?.BoundingBox);
        }
    }
}
