using System.Threading.Tasks;

namespace CustomVisionInteraction.Prediction
{
    public class PredictionFacade : IPrediction
    {
        private readonly PillClassification _pillClassification;
        private readonly ColorAnalyzer _colorAnalyzer;

        public PredictionFacade(IPredictionContext classificationContext, IPredictionContext detectionContext, IContext visionContext)
        {
            _pillClassification = new PillClassification(new PillClassificationCommunication(classificationContext));
            _colorAnalyzer = new ColorAnalyzer(new ComputerVisionCommunication(visionContext), new PillDetection(new PillDetectionCommunication(detectionContext)));
        }

        public async Task<PredictionResult> PredictImage(byte[] image)
        {
            var (hasClassification, tags) = await _pillClassification.GetClassification(image);
            if (!hasClassification)
                return PredictionResult.GetNoSuccess();

            var color = await _colorAnalyzer.GetColor(image);

            return PredictionResult.FromSuccess(tags, color);
        }
    }
}
