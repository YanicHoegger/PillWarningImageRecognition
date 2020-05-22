using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models;

namespace CustomVisionInteraction.ColorAnalyzer
{
    public interface ICroppingService
    {
        byte[] CropImage(byte[] image, BoundingBox boundingBox);
    }
}
