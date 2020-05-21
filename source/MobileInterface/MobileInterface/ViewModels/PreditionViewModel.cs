using MobileInterface.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.Threading.Tasks;
using WebInterface.Shared;

namespace MobileInterface.ViewModels
{
    public class PreditionViewModel : BaseViewModel
    {
        private readonly IPredictionService _predictionService;

        public PreditionViewModel(IPredictionService predictionService)
        {
            _predictionService = predictionService;
        }

        public bool CanTakePhoto => CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported && !IsBusy;
        public bool CanPickPhoto => CrossMedia.Current.IsPickPhotoSupported && !IsBusy;

        public async Task<PredictionResult> PredictFromTakePhoto()
        {
            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions());
            return await PredictFromPhoto(file);
        }

        public async Task<PredictionResult> PredictFromPickPhoto()
        {
            var file = await CrossMedia.Current.PickPhotoAsync();
            return await PredictFromPhoto(file);
        }

        private async Task<PredictionResult> PredictFromPhoto(MediaFile file)
        {
            if (file == null)
                return null;

            IsBusyInternal = true;

            var fileName = file.Path.Replace(file.AlbumPath, "");
            var predictionResult = await _predictionService.Predict(file.GetStream(), fileName);

            IsBusyInternal = false;

            return predictionResult;
        }

        private bool IsBusyInternal
        {
            get => IsBusy;
            set
            {
                IsBusy = value;
                OnPropertyChanged(nameof(CanTakePhoto));
                OnPropertyChanged(nameof(CanPickPhoto));
            }
        }
    }
}
