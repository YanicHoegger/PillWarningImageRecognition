using System.Collections.Generic;
using System.Linq;
using WebInterface.Shared;

namespace MobileInterface.ViewModels
{
    public class PredicitonResultViewModel
    {
        public PredicitonResultViewModel(PredictionResult predictionResult)
        {
            Items = predictionResult.SameInprint.Select(x => new PredictionResultItemViewModel(x));
        }

        public IEnumerable<PredictionResultItemViewModel> Items { get; }
    }
}
