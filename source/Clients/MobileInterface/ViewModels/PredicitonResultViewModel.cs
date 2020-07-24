using Clients.Shared;
using System.Collections.Generic;
using System.Linq;

namespace MobileInterface.ViewModels
{
    public class PredicitonResultViewModel
    {
        public PredicitonResultViewModel(PredictionResult predictionResult)
        {
            //TODO: 
            //Items = predictionResult.SameInprint.Select(x => new PillWarningViewModel(x));
            Items = Enumerable.Empty<PillWarningViewModel>();
        }

        public IEnumerable<PillWarningViewModel> Items { get; }
    }
}
