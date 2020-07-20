using Clients.Shared;
using System.Collections.Generic;
using System.Linq;

namespace MobileInterface.ViewModels
{
    public class PredicitonResultViewModel
    {
        public PredicitonResultViewModel(PredictionResult predictionResult)
        {
            Items = predictionResult.SameInprint.Select(x => new PillWarningViewModel(x));
        }

        public IEnumerable<PillWarningViewModel> Items { get; }
    }
}
