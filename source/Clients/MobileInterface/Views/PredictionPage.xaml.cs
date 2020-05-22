using MobileInterface.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileInterface.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PredictionPage : ContentPage
    {
        readonly PreditionViewModel _viewModel;

        public PredictionPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = Startup.ServiceProvider.GetService<PreditionViewModel>();
        }

        async void OnTakePhoto(object sender, EventArgs args)
        {
            var prediciton = await _viewModel.PredictFromTakePhoto();
            await Navigate(prediciton);
        }

        async void OnPickPhoto(object sender, EventArgs args)
        {
            var prediciton = await _viewModel.PredictFromPickPhoto();
            await Navigate(prediciton);
        }

        async Task Navigate(Clients.Shared.PredictionResult predictionResult)
        {
            if (predictionResult == null)
                return;

            await Navigation.PushAsync(new PredictionResult(new PredicitonResultViewModel(predictionResult)));
        }
    }
}