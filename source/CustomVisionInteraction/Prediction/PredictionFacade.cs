using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace CustomVisionInteraction.Prediction
{
    public class PredictionFacade : IPrediction
    {
        private readonly PillClassification _pillClassification;
        private readonly PillDetection _pillDetection;
        private readonly ColorAnalyzer _colorAnalyzer;

        public PredictionFacade(IPredictionContext classificationContext, IPredictionContext detectionContext, IContext visionContext)
        {
            _pillClassification = new PillClassification(new PillClassificationCommunication(classificationContext));
            _pillDetection = new PillDetection(new PillDetectionCommunication(detectionContext));
            _colorAnalyzer = new ColorAnalyzer(new ComputerVisionCommunication(visionContext));
        }

        public async Task<PredictionResult> PredictImage(byte[] image)
        {
            var (hasClassification, tags) = await _pillClassification.GetClassification(image);
            if (!hasClassification)
                return PredictionResult.GetNoSuccess();

            var (hasDetection, boundingBox) = await _pillDetection.GetBestDetection(image);

            Color color;
            if(hasDetection)
            {
                var croppedImage = new CroppingService().CropImage(image, boundingBox);
                color = await _colorAnalyzer.GetColor(croppedImage);
            }
            else
            {
                color = await _colorAnalyzer.GetColor(image);
            }

            return PredictionResult.FromSuccess(tags, color);
        }
    }
}
