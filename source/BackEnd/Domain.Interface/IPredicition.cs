using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface IPredicition
    {
        Task<IPredictionResult> Predict(byte[] image);
    }
}
