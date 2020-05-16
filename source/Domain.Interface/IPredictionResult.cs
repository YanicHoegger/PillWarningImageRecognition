using DatabaseInteraction.Interface;
using System.Collections.Generic;

namespace Domain.Interface
{
    public interface IPredictionResult
    {
        IEnumerable<DrugCheckingSource> TagFindings { get; }
        IEnumerable<DrugCheckingSource> ColorFindings { get; }
    }
}
