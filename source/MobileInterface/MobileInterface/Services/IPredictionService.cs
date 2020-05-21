using System.IO;
using System.Threading.Tasks;
using WebInterface.Shared;

namespace MobileInterface.Services
{
    public interface IPredictionService
    {
        Task<PredictionResult> Predict(Stream image, string name);
    }
}
