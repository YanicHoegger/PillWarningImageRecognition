using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageInteraction.Interface;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models;

namespace ImageInteraction.Classification
{
    public class PillClassification : IClassifier
    {
        private readonly IClassificationContext _classificationContext;

        public PillClassification(IClassificationContext classificationContext)
        {
            _classificationContext = classificationContext;
        }

        public async Task<IImageClassificationResult> GetImageClassification(byte[] image)
        {
            using var endpoint = CreateEndPoint();

            var result = await endpoint.ClassifyImageAsync(_classificationContext.ProjectId, _classificationContext.PublisherModelName, new MemoryStream(image));

            return Convert(result);
        }

        public async Task<IImageClassificationResult> GetImageClassificationNoStore(byte[] image)
        {
            using var endpoint = CreateEndPoint();

            var result = await endpoint.ClassifyImageWithNoStoreAsync(_classificationContext.ProjectId, _classificationContext.PublisherModelName, new MemoryStream(image));

            return Convert(result);
        }

        private CustomVisionPredictionClient CreateEndPoint()
        {
            return new CustomVisionPredictionClient
            {
                ApiKey = _classificationContext.Key,
                Endpoint = _classificationContext.EndPoint,
            };
        }

        private static IImageClassificationResult Convert(ImagePrediction imagePrediction)
        {
            return new ImageClassificationResult(imagePrediction
                .Predictions
                .Select(x => new TagClassificationResult(x.TagName, x.Probability)));
        }
    }
}
