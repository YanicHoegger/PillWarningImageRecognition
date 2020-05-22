using Microsoft.Extensions.Hosting;
using Plugin.Media;
using System.Threading;
using System.Threading.Tasks;

namespace MobileInterface.Services
{
    public class MediaPluginInitService : IHostedService
    {
        private bool _isInitialized;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (_isInitialized)
                return;

            await CrossMedia.Current.Initialize();
            _isInitialized = true;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
