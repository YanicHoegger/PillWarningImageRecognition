using System.Collections.Generic;
using System.Threading.Tasks;
using ImageInteraction.Interface;

namespace Domain.Prediction
{
    public interface IPillRecognizer
    {
        Task<bool> IsPill(byte[] image);
        bool IsPill(IEnumerable<ITagClassificationResult> classificationResult);
    }
}
