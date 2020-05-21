using MobileInterface.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileInterface.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PredictionResult : ContentPage
    {
        public PredictionResult(PredicitonResultViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        async void OnImageTapped(object sender, EventArgs args)
        {
            if ((sender as BindableObject)?.BindingContext is PredictionResultItemViewModel item)
            {
                await Navigation.PushAsync(new PillWarning(await PillWarningViewModelFactory.Create(item.PdfUrl)));
            }
        }
    }
}