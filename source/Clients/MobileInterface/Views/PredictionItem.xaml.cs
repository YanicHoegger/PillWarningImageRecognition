using System;
using MobileInterface.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileInterface.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PredictionItem
    {
        public PredictionItem()
        {
            InitializeComponent();
        }

        async void OnImageTapped(object sender, EventArgs args)
        {
            if ((sender as BindableObject)?.BindingContext is PillWarningViewModel item)
            {
                await Navigation.PushAsync(new PillWarning(item));
            }
        }
    }
}