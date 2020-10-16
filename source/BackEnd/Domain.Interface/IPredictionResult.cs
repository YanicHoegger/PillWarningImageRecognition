using System.Collections.Generic;

namespace Domain.Interface
{
    public interface IPredictionResult
    {
        bool IsPill { get; }
        IEnumerable<IFinding> TagFindings { get; }
        IEnumerable<IPillWarning> ColorFindings { get; }
    }
}
