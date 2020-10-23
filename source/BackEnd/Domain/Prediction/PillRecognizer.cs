using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Interface;
using ImageInteraction.Interface;

namespace Domain.Prediction
{
    public class PillRecognizer : IPillRecognizer
    {
        private readonly IClassifier _classifier;
        private readonly IProbabilityToLikelinessConverter _converter;

        public PillRecognizer(IClassifier classifier, IProbabilityToLikelinessConverter converter)
        {
            _classifier = classifier;
            _converter = converter;
        }

        public async Task<bool> IsPill(byte[] image)
        {
            var classificationResult = await _classifier.GetImageClassification(image);

            return IsPill(classificationResult.TagClassifications);
        }

        public bool IsPill(IEnumerable<ITagClassificationResult> classificationResult)
        {
            var pillLikeliness = GetPillLikeliness(classificationResult);

            return pillLikeliness >= Likeliness.Maybe;
        }

        private Likeliness GetPillLikeliness(IEnumerable<ITagClassificationResult> classificationResult)
        {
            var pillClassification = classificationResult.SingleOrDefault(x => x.TagName.Equals(Constants.PillTag));

            return pillClassification == null
                ? Likeliness.NotAtAll
                : _converter.Convert(pillClassification.Probability);
        }
    }
}
