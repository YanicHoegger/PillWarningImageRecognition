using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageInteraction.Interface
{
    public interface IDetector
    {
        Task<IEnumerable<IDetectionResult>> GetDetection(byte[] image);
    }
}
