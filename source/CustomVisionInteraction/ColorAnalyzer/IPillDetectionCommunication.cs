using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models;
using System.IO;
using System.Threading.Tasks;

namespace CustomVisionInteraction.Interface
{
    public interface IPillDetectionCommunication
    {
        Task<ImagePrediction> DetectPill(Stream image);
    }
}
