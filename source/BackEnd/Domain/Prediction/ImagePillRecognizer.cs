using System.Threading.Tasks;
using ImageInteraction.Interface;

namespace Domain.Prediction
{
    public class ImagePillRecognizer : ClassificationPillRecognizer, IImagePillRecognizer
    {
        private readonly IClassifier _classifier;
        private readonly IPredictedImagesManager _predictedImagesManager;

        public ImagePillRecognizer(IClassifier classifier, IPredictedImagesManager predictedImagesManager, IProbabilityToLikelinessConverter converter) 
            : base(converter)
        {
            _classifier = classifier;
            _predictedImagesManager = predictedImagesManager;
        }

        public async Task<bool> IsPill(byte[] image)
        {
            var classificationResult = await _classifier.GetImageClassification(image);
            await _predictedImagesManager.DeletePredictedImages(image);

            return IsPill(classificationResult.TagClassifications);
        }
    }
}
