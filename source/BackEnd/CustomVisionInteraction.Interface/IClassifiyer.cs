using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomVisionInteraction.Interface
{
    public interface IClassifier
    {
        Task<IEnumerable<IClassificationResult>> GetImageClassification(byte[] image);
    }
}
