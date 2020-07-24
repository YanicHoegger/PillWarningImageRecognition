using System.Collections.Generic;

namespace Domain.Interface
{
    public interface IPredictionResult
    {
        Likeliness IsPill { get; }
        IEnumerable<IFinding> TagFindings { get; }
        IEnumerable<IPillWarning> ColorFindings { get; }
    }
}
