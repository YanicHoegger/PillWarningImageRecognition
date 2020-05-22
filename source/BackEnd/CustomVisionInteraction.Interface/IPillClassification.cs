using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomVisionInteraction.Interface
{
    public interface IPillClassification
    {
        Task<(bool hasClassification, IEnumerable<string> tags)> GetClassification(byte[] image);
    }
}
