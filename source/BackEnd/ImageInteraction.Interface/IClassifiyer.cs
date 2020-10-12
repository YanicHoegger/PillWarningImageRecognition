using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImageInteraction.Interface
{
    public interface IClassifier
    {
        Task<IEnumerable<IClassificationResult>> GetImageClassification(byte[] image);
    }
}
