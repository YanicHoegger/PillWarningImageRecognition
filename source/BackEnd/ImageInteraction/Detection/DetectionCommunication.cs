using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models;

namespace ImageInteraction.Detection
{
    public class DetectionCommunication : IDetectionCommunication
    {
        private readonly IClassificationContext _classificationContext;

        public DetectionCommunication(IClassificationContext classificationContext)
        {
            _classificationContext = classificationContext;
        }

        public async Task<ImagePrediction> DetectPill(Stream image)
        {
            using var endpoint = new CustomVisionPredictionClient()
            {
                ApiKey = _classificationContext.Key,
                Endpoint = _classificationContext.EndPoint,
            };
            return await endpoint.DetectImageAsync(_classificationContext.ProjectId, _classificationContext.PublisherModelName, image);
        }
    }
}
