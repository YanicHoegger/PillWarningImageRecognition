using System.Collections.Generic;
using System.Linq;
using Domain.Interface;

namespace Domain.Prediction
{
    public class PredictionResult : IPredictionResult
    {
        private PredictionResult(bool isPill, IEnumerable<IFinding> tagFindings, IEnumerable<IPillWarning> colorFindings)
        {
            IsPill = isPill;
            TagFindings = tagFindings;
            ColorFindings = colorFindings;
        }

        public bool IsPill { get; }
        public IEnumerable<IFinding> TagFindings { get; }
        public IEnumerable<IPillWarning> ColorFindings { get; }

        public static PredictionResult NoPillResult()
        {
            return new PredictionResult(false, Enumerable.Empty<IFinding>(), Enumerable.Empty<IPillWarning>());
        }

        public static PredictionResult FromSuccess(IEnumerable<IFinding> tagFindings, IEnumerable<IPillWarning> colorFindings)
        {
            return new PredictionResult(true, tagFindings, colorFindings);
        }
    }
}
