using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models;

namespace ImageInteraction.Classification
{
    public class PillClassificationCommunication : IPillClassificationCommunication
    {
        private readonly IClassificationContext _classificationContext;

        public PillClassificationCommunication(IClassificationContext classificationContext)
        {
            _classificationContext = classificationContext;
        }

        public async Task<ImagePrediction> ClassifyImage(Stream image)
        {
            using var endpoint = new CustomVisionPredictionClient
            {
                ApiKey = _classificationContext.Key,
                Endpoint = _classificationContext.EndPoint,
            };
            return await endpoint.ClassifyImageAsync(_classificationContext.ProjectId, _classificationContext.PublisherModelName, image);
        }
    }
}
