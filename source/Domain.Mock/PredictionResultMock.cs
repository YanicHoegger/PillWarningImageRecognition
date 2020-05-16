using DatabaseInteraction.Interface;
using Domain.Interface;
using System.Collections.Generic;

namespace Domain.Mock
{
    public class PredictionResultMock : IPredictionResult
    {
        public IEnumerable<DrugCheckingSource> TagFindings { get; set; }

        public IEnumerable<DrugCheckingSource> ColorFindings { get; set; }
    }
}
