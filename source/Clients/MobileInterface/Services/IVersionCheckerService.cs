using System.Threading.Tasks;

namespace MobileInterface.Services
{
    public interface IVersionCheckerService
    {
        Task<bool> GetIsCorrectServerVersion();
    }
}
