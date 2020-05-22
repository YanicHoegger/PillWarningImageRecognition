
using MobileInterface.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileInterface.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PillWarning : ContentPage
    {
        public PillWarning(PillWarningViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}