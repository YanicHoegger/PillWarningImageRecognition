using DatabaseInteraction;
using System.Collections.Generic;

namespace Domain
{
    public class PredictionResult
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
