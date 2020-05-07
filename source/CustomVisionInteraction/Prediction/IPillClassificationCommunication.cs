using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models;
using System.IO;
using System.Threading.Tasks;

namespace CustomVisionInteraction.Prediction
{
    public interface IPillClassificationCommunication
    {
        Task<ImagePrediction> ClassifyImage(Stream image);
    }
}
