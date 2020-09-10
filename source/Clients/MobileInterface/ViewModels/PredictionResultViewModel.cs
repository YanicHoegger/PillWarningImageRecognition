using Clients.Shared;
using System.Collections.Generic;
using System.Linq;

namespace MobileInterface.ViewModels
{
    public class PredictionResultViewModel
    {
        private static readonly PredictionResultHelper _predictionResultHelper = new PredictionResultHelper();

        public PredictionResultViewModel(PredictionResult predictionResult)
        {
            IsPill = _predictionResultHelper.IsPill(predictionResult);

            if(IsPill)
            {
                Items = predictionResult
                    .TagFindings
                    .Select(Convert)
                    .Append(new PredictionItemViewModel("Pillen mit gleicher Farbe", predictionResult
                        .ColorFindings
                        .Select(x => new PillWarningViewModel(x))));
            }
            else
            {
                Items = Enumerable.Empty<PredictionItemViewModel>();
            }
        }

        public bool IsPill { get; }

        public IEnumerable<PredictionItemViewModel> Items { get; }

        public string NoPillResponse => _predictionResultHelper.NoPillResponse;

        private static PredictionItemViewModel Convert(Finding finding)
        {
            var title = _predictionResultHelper.ConvertLikeliness(finding.Likeliness);
            var pillWarningViewModels = finding.PillWarnings.Select(x => new PillWarningViewModel(x));

            return new PredictionItemViewModel(title, pillWarningViewModels);
        }
    }
}
