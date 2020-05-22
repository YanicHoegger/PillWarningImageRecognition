using System.IO;
using System.Threading.Tasks;
using Clients.Shared;

namespace WebInterface.Services
{
    public interface IPillWarningService
    {
        Task<PredictionResult> GetPillWarnings(Stream image);
    }
}
