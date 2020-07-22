using CustomVisionInteraction.Interface;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CustomVisionInteraction.ColorAnalyzer
{
    public class PillDetection : IDetector
    {
        private readonly IPillDetectionCommunication _pillDetectionCommunication;

        public PillDetection(IPillDetectionCommunication pillDetectionCommunication)
        {
            _pillDetectionCommunication = pillDetectionCommunication;
        }

        public async Task<IEnumerable<IDetectionResult>> GetDetection(byte[] image)
        {
            var result = await _pillDetectionCommunication.DetectPill(new MemoryStream(image));

            return result.Predictions.Select(x => new DetectionResult(new BoundingBox(x.BoundingBox), x.Probability));
        }
    }
}
