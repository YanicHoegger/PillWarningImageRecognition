using System.Collections.Generic;
using ImageInteraction.Interface;

namespace Domain.Prediction
{
    public interface IClassificationPillRecognizer
    {
        bool IsPill(IEnumerable<ITagClassificationResult> classificationResult);
    }
}
