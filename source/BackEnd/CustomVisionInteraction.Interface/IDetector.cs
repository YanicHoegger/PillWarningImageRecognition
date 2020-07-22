using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomVisionInteraction.Interface
{
    public interface IDetector
    {
        Task<IEnumerable<IDetectionResult>> GetDetection(byte[] image);
    }
}
