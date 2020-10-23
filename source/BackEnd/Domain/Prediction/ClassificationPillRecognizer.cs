using System.Collections.Generic;
using System.Linq;
using Domain.Interface;
using ImageInteraction.Interface;

namespace Domain.Prediction
{
    public class ClassificationPillRecognizer : IClassificationPillRecognizer
    {
        private readonly IProbabilityToLikelinessConverter _converter;

        public ClassificationPillRecognizer(IProbabilityToLikelinessConverter converter)
        {
            _converter = converter;
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
