using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MobileInterface.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "Info";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://www.eve-rave.ch/"));
            ShareLogFileCommand = new Command(async  () => await ShareLogFile());
        }

        public ICommand OpenWebCommand { get; }

        public ICommand ShareLogFileCommand { get; }

        private static async Task ShareLogFile()
        {
            var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var allLogFiles = Directory.GetFiles(basePath, "log*.txt", SearchOption.AllDirectories);

            if (allLogFiles.Length > 0)
            {
                await Share.RequestAsync(new ShareFileRequest
                {
                    Title = "Log File",
                    File = new ShareFile(allLogFiles[0])
                });
            }
        }
    }
}