using CustomVisionInteraction.Interface;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models;
using System.IO;
using System.Threading.Tasks;

namespace CustomVisionInteraction.Prediction
{
    public class PillClassificationCommunication : IPillClassificationCommunication
    {
        private readonly IPredictionContext _predictionContext;

        public PillClassificationCommunication(IPredictionContext predictionContext)
        {
            _predictionContext = predictionContext;
        }

        public async Task<ImagePrediction> ClassifyImage(Stream image)
        {
            using var endpoint = new CustomVisionPredictionClient()
            {
                ApiKey = _predictionContext.Key,
                Endpoint = _predictionContext.EndPoint,
            };
            return await endpoint.ClassifyImageAsync(_predictionContext.ProjectId, _predictionContext.PublisherModelName, image);
        }
    }
}
