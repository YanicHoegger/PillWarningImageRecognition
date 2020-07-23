using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageInteraction.Interface;

namespace ImageInteraction.Detection
{
    public class Detector : IDetector
    {
        private readonly IDetectionCommunication _detectionCommunication;

        public Detector(IDetectionCommunication detectionCommunication)
        {
            _detectionCommunication = detectionCommunication;
        }

        public async Task<IEnumerable<IDetectionResult>> GetDetection(byte[] image)
        {
            var result = await _detectionCommunication.DetectPill(new MemoryStream(image));

            return result.Predictions.Select(x => new DetectionResult(new BoundingBox(x.BoundingBox), x.Probability));
        }
    }
}
