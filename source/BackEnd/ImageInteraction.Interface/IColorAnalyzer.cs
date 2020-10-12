using System.Drawing;
using System.Threading.Tasks;

namespace ImageInteraction.Interface
{
    public interface IColorAnalyzer
    {
        Task<Color> GetColor(byte[] image);
    }
}
