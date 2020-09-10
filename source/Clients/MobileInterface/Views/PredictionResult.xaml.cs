using Xamarin.Forms.Xaml;

namespace MobileInterface.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PredictionResult
    {
        public PredictionResult(object viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}