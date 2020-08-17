using System.Collections.Generic;

namespace MobileInterface.ViewModels
{
    public class PredictionItemViewModel
    {
        public PredictionItemViewModel(string title, IEnumerable<PillWarningViewModel> pillWarnings)
        {
            Title = title;
            PillWarnings = pillWarnings;
        }

        public string Title { get; }
        public IEnumerable<PillWarningViewModel> PillWarnings { get; }
    }
}
