using CustomVisionInteraction.Interface;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models;
using System.IO;
using System.Threading.Tasks;

namespace CustomVisionInteraction.ColorAnalyzer
{
    public class PillDetectionCommunication : IPillDetectionCommunication
    {
        private readonly IPredictionContext _predictionContext;

        public PillDetectionCommunication(IPillDetectionContext predictionContext)
        {
            _predictionContext = predictionContext;
        }

        public async Task<ImagePrediction> DetectPill(Stream image)
        {
            using var endpoint = new CustomVisionPredictionClient()
            {
                ApiKey = _predictionContext.Key,
                Endpoint = _predictionContext.EndPoint,
            };
            return await endpoint.DetectImageAsync(_predictionContext.ProjectId, _predictionContext.PublisherModelName, image);
        }
    }
}
