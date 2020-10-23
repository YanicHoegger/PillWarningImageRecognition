using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageInteraction.Interface;

namespace ImageInteraction.Classification
{
    public class PillClassification : IClassifier
    {
        private readonly IPillClassificationCommunication _pillClassificationCommunication;

        public PillClassification(IPillClassificationCommunication pillClassificationCommunication)
        {
            _pillClassificationCommunication = pillClassificationCommunication;
        }

        public async Task<IImageClassificationResult> GetImageClassification(byte[] image)
        {
            var result = await _pillClassificationCommunication.ClassifyImage(new MemoryStream(image));

            return new ImageClassificationResult(result
                .Predictions
                .Select(x => new TagClassificationResult(x.TagName, x.Probability)));
        }
    }
}
