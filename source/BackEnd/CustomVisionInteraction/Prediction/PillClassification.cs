using CustomVisionInteraction.Interface;
using CustomVisionInteraction.Prediction;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CustomVisionInteraction
{
    public class PillClassification : IPillClassification
    {
        private readonly IPillClassificationCommunication _pillClassificationCommunication;

        private const double _minimumPropabilityForPill = 0.9;
        private const double _minimumTagProbabilit = 0.2;
        private const string _pillTag = "Pill";

        public PillClassification(IPillClassificationCommunication pillClassificationCommunication)
        {
            _pillClassificationCommunication = pillClassificationCommunication;
        }

        public async Task<(bool hasClassification, IEnumerable<string> tags)> GetClassification(byte[] image)
        {
            var result = await _pillClassificationCommunication.ClassifyImage(new MemoryStream(image));

            var pillPrediction = result.Predictions.SingleOrDefault(x => x.TagName.Equals(_pillTag));
            if (pillPrediction == null || pillPrediction.Probability < _minimumPropabilityForPill)
                return (false, Enumerable.Empty<string>());

            var tags = result
                .Predictions
                .Where(x => !x.TagName.Equals(_pillTag) && x.Probability > _minimumTagProbabilit)
                .Select(x => x.TagName);

            return (true, tags);
        }
    }
}
