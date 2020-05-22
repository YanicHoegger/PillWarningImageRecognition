using Clients.Shared;
using System.IO;
using System.Threading.Tasks;

namespace MobileInterface.Services
{
    public interface IPredictionService
    {
        Task<PredictionResult> Predict(Stream image, string name);
    }
}
