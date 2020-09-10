using Clients.Shared;
using MobileInterface.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MobileInterface.ViewModels
{
    public class PredictionViewModel : BaseViewModel
    {
        private const string _fileName = "PillWarningPredictionImage.jpg";
        private readonly IPredictionService _predictionService;
        private readonly IVersionCheckerService _versionCheckerService;

        public PredictionViewModel(IPredictionService predictionService, IVersionCheckerService versionCheckerService)
        {
            _predictionService = predictionService;
            _versionCheckerService = versionCheckerService;
        }

        public void Init()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Connectivity.ConnectivityChanged += CheckVersion;
            }
            else
            {
                CheckVersion();
            }
        }

        public bool CanTakePhoto => CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported && !IsBusy;
        public bool CanPickPhoto => CrossMedia.Current.IsPickPhotoSupported && !IsBusy;

        public bool NotRightVersion { get; private set; }

        public async Task<PredictionResult> PredictFromTakePhoto()
        {
            SetIsBusy(true);

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions());
            return await PredictFromPhoto(file);
        }

        public async Task<PredictionResult> PredictFromPickPhoto()
        {
            SetIsBusy(true);

            var file = await CrossMedia.Current.PickPhotoAsync();
            return await PredictFromPhoto(file);
        }

        private async Task<PredictionResult> PredictFromPhoto(MediaFile file)
        {
            if (file == null)
            {
                SetIsBusy(false);
                return null;
            }

            var predictionResult = await _predictionService.Predict(file.GetStream(), _fileName);

            SetIsBusy(false);

            return predictionResult;
        }

        private void SetIsBusy(bool value)
        {
            IsBusy = value;
            OnPropertyChanged(nameof(CanTakePhoto));
            OnPropertyChanged(nameof(CanPickPhoto));
        }

        private void CheckVersion()
        {
            //TODO: Handle async
            NotRightVersion = !_versionCheckerService.GetIsCorrectServerVersion().Result;
            OnPropertyChanged(nameof(NotRightVersion));
        }

        private void CheckVersion(object sender, ConnectivityChangedEventArgs e)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                return;
            }

            CheckVersion();
            Connectivity.ConnectivityChanged -= CheckVersion;
        }
    }
}
