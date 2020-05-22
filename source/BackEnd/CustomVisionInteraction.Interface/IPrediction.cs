using System.Threading.Tasks;

namespace CustomVisionInteraction.Interface
{
    public interface IPrediction
    {
        Task<IPredictionResult> PredictImage(byte[] image);
    }
}
