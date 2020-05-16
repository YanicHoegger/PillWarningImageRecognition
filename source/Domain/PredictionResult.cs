using DatabaseInteraction.Interface;
using Domain.Interface;
using System.Collections.Generic;

namespace Domain
{
    public class PredictionResult : IPredictionResult
    {
        public PredictionResult(IEnumerable<DrugCheckingSource> tagFindings, IEnumerable<DrugCheckingSource> colorFindings)
        {
            TagFindings = tagFindings;
            ColorFindings = colorFindings;
        }

        public IEnumerable<DrugCheckingSource> TagFindings { get; }
        public IEnumerable<DrugCheckingSource> ColorFindings { get; }
    }
}
