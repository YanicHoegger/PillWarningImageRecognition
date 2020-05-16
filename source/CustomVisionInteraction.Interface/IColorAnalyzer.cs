using System.Drawing;
using System.Threading.Tasks;

namespace CustomVisionInteraction.Interface
{
    public interface IColorAnalyzer
    {
        Task<Color> GetColor(byte[] image);
    }
}
