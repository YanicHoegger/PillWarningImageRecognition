using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models;
using System.Threading.Tasks;

namespace CustomVisionInteraction.Interface
{
    public interface IPillDetection
    {
        Task<(bool hasDetection, BoundingBox boundingBox)> GetBestDetection(byte[] image);
    }
}
