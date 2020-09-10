using System.Collections.Generic;
using Domain.Interface;

namespace Domain.Prediction
{
    public class PredictionResult : IPredictionResult
    {
        public PredictionResult(Likeliness isPill, IEnumerable<IFinding> tagFindings, IEnumerable<IPillWarning> colorFindings)
        {
            IsPill = isPill;
            TagFindings = tagFindings;
            ColorFindings = colorFindings;
        }

        public Likeliness IsPill { get; }
        public IEnumerable<IFinding> TagFindings { get; }
        public IEnumerable<IPillWarning> ColorFindings { get; }
    }
}
