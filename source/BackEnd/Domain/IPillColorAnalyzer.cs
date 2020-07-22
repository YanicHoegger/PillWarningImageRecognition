using System.Drawing;
using System.Threading.Tasks;

namespace Domain
{
    public interface IPillColorAnalyzer
    {
        Task<Color> GetColor(byte[] image);
    }
}
