using System.Drawing;
using System.Threading.Tasks;

namespace Domain.Prediction
{
    public interface IPillColorAnalyzer
    {
        Task<Color> GetColor(byte[] image);
    }
}
