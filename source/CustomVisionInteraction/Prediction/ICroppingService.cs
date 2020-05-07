using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models;

namespace CustomVisionInteraction.Prediction
{
    public interface ICroppingService
    {
        byte[] CropImage(byte[] image, BoundingBox boundingBox);
    }
}
