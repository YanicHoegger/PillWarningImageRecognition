using Clients.Shared;
using MobileInterface.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.Threading.Tasks;

namespace MobileInterface.ViewModels
{
    public class PreditionViewModel : BaseViewModel
    {
        private const string _fileName = "PillWarningPredictionImage.jpg";
        private readonly IPredictionService _predictionService;

        public PreditionViewModel(IPredictionService predictionService)
        {
            _predictionService = predictionService;
        }

        public bool CanTakePhoto => CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported && !IsBusy;
        public bool CanPickPhoto => CrossMedia.Current.IsPickPhotoSupported && !IsBusy;

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
    }
}
