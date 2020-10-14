using Microsoft.Extensions.DependencyInjection;
using MobileInterface.Services;

namespace MobileInterface.ViewModels
{
    public class VersionViewModel : BaseViewModel
    {
        private readonly IVersionCheckerService _versionCheckerService;

        public VersionViewModel()
        {
            _versionCheckerService = Startup.ServiceProvider.GetService<IVersionCheckerService>();
            _versionCheckerService.VersionChecked += VersionCheckerServiceOnVersionChecked;
        }

        public bool IsWrongVersion =>
            _versionCheckerService.IsVersionChecked && !_versionCheckerService.IsVersionCorrect;

        private void VersionCheckerServiceOnVersionChecked()
        {
            OnPropertyChanged(nameof(IsWrongVersion));
        }
    }
}
