using Clients.Shared;

namespace MobileInterface.ViewModels
{
    public class PredictionResultItemViewModel
    {
        private readonly PillWarning _pillWarning;

        public PredictionResultItemViewModel(PillWarning pillWarning)
        {
            _pillWarning = pillWarning;
        }

        public byte[] Image => _pillWarning.Image;

        public string PdfUrl => _pillWarning.PdfLocation;
    }
}
