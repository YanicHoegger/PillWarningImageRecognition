using Clients.Shared;
using System.IO;
using System.Threading.Tasks;

namespace WebInterface.Services
{
    public interface IPillWarningService
    {
        Task<PredictionResult> GetPillWarnings(Stream image);
    }
}
