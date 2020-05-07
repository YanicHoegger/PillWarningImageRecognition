using System.IO;
using System.Threading.Tasks;

namespace CustomVisionInteraction.Prediction
{
    public interface IPrediction
    {
        Task<PredictionResult> PredictImage(byte[] image);
    }
}
