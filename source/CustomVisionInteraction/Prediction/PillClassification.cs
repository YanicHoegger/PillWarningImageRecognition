using CustomVisionInteraction.Prediction;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CustomVisionInteraction
{
    public class PillClassification
    {
        private readonly IPillClassificationCommunication _pillClassificationCommunication;

        private const double MinimumPropabilityForPill = 0.9;
        private const double MinimumTagProbabilit = 0.2;
        private const string PillTag = "Pill";

        public PillClassification(IPillClassificationCommunication pillClassificationCommunication)
        {
            _pillClassificationCommunication = pillClassificationCommunication;
        }

        public async Task<(bool hasClassification, IEnumerable<string> tags)> GetClassification(byte[] image)
        {
            var result = await _pillClassificationCommunication.ClassifyImage(new MemoryStream(image));

            var pillPrediction = result.Predictions.SingleOrDefault(x => x.TagName.Equals(PillTag));
            if (pillPrediction == null || pillPrediction.Probability < MinimumPropabilityForPill)
                return (false, Enumerable.Empty<string>());

            var tags = result
                .Predictions
                .Where(x => !x.TagName.Equals(PillTag) && x.Probability > MinimumTagProbabilit)
                .Select(x => x.TagName);

            return (true, tags);
        }
    }
}
