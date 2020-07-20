using Clients.Shared;

namespace MobileInterface.ViewModels
{
    public class PillWarningViewModel
    {
        public PillWarningViewModel(PillWarning pillWarning)
        {
            PillWarning = pillWarning;
        }

        public PillWarning PillWarning { get; }
    }
}
