using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IPredictedImagesCleaner
    {
        Task CleanPredictions();
    }
}
