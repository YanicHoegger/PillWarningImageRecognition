using MobileInterface.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms.Xaml;

namespace MobileInterface.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PredictionPage
    {
        readonly PreditionViewModel _viewModel;

        public PredictionPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = Startup.ServiceProvider.GetService<PreditionViewModel>();
        }

        async void OnTakePhoto(object sender, EventArgs args)
        {
            var predictionResult = await _viewModel.PredictFromTakePhoto();
            await Navigate(predictionResult);
        }

        async void OnPickPhoto(object sender, EventArgs args)
        {
            var predictionResult = await _viewModel.PredictFromPickPhoto();
            await Navigate(predictionResult);
        }

        async Task Navigate(Clients.Shared.PredictionResult predictionResult)
        {
            if (predictionResult == null)
                return;

            await Navigation.PushAsync(new PredictionResult(new PredictionResultViewModel(predictionResult)));
        }
    }
}