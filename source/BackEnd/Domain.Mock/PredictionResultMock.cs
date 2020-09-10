using Domain.Interface;
using System.Collections.Generic;

namespace Domain.Mock
{
    public class PredictionResultMock : IPredictionResult
    {
        public Likeliness IsPill { get; set; }
        public IEnumerable<IFinding> TagFindings { get; set; }
        public IEnumerable<IPillWarning> ColorFindings { get; set; }
    }
}
