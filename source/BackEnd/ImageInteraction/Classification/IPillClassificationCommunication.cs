using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models;

namespace ImageInteraction.Classification
{
    public interface IPillClassificationCommunication
    {
        Task<ImagePrediction> ClassifyImage(Stream image);
    }
}
