using System.Threading.Tasks;
using ImageInteraction.Interface;

namespace Domain.Prediction
{
    public class ImagePillRecognizer : ClassificationPillRecognizer, IImagePillRecognizer
    {
        private readonly IClassifier _classifier;

        public ImagePillRecognizer(IClassifier classifier, IProbabilityToLikelinessConverter converter) 
            : base(converter)
        {
            _classifier = classifier;
        }

        public async Task<bool> IsPill(byte[] image)
        {
            var classificationResult = await _classifier.GetImageClassificationNoStore(image);
            return IsPill(classificationResult.TagClassifications);
        }
    }
}
