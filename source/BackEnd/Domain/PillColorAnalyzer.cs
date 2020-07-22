using CustomVisionInteraction.Interface;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Domain
{
    public class PillColorAnalyzer : IPillColorAnalyzer
    {
        private const double _minimumProbabilityPill = 0.9;

        private readonly IDetector _detector;
        private readonly ICroppingService _croppingService;
        private readonly IColorAnalyzer _colorAnalyzer;

        public PillColorAnalyzer(IDetector detector, ICroppingService croppingService, IColorAnalyzer colorAnalyzer)
        {
            _detector = detector;
            _croppingService = croppingService;
            _colorAnalyzer = colorAnalyzer;
        }

        public async Task<Color> GetColor(byte[] image)
        {
            var croppedImage = await CropImage(image);

            return await _colorAnalyzer.GetColor(croppedImage);
        }

        private async Task<byte[]> CropImage(byte[] image)
        {
            var detectorResult = await _detector.GetDetection(image);

            var bestDetection = detectorResult.OrderBy(x => x.Probability).Last();

            return bestDetection.Probability > _minimumProbabilityPill 
                ? _croppingService.CropImage(image, bestDetection.BoundingBox) 
                : image;
        }
    }
}
