using CustomVisionInteraction.Interface;
using System.Threading.Tasks;

namespace CustomVisionInteraction.Prediction
{
    public class PredictionFacade : IPrediction
    {
        private readonly IPillClassification _pillClassification;
        private readonly IColorAnalyzer _colorAnalyzer;

        public PredictionFacade(IPillClassification pillClassification, IColorAnalyzer colorAnalyzer)
        {
            _pillClassification = pillClassification;
            _colorAnalyzer = colorAnalyzer;
        }

        public async Task<IPredictionResult> PredictImage(byte[] image)
        {
            var (hasClassification, tags) = await _pillClassification.GetClassification(image);
            if (!hasClassification)
                return PredictionResult.GetNoSuccess();

            var color = await _colorAnalyzer.GetColor(image);

            return PredictionResult.FromSuccess(tags, color);
        }
    }
}
