using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models;

namespace ImageInteraction.Detection
{
    public interface IDetectionCommunication
    {
        Task<ImagePrediction> DetectPill(Stream image);
    }
}
