using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Clients.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Xamarin.Essentials;

namespace MobileInterface.Services
{
    public class VersionCheckerService : IVersionCheckerService, IHostedService
    {
        private const string _uriConfiguration = "VersionUri";

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public VersionCheckerService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public bool IsVersionChecked { get; private set; }
        public bool IsVersionCorrect { get; private set; }

        public event Action VersionChecked;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Connectivity.ConnectivityChanged += CheckVersion;
            }
            else
            {
                await CheckVersion(cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Connectivity.ConnectivityChanged -= CheckVersion;

            return Task.CompletedTask;
        }

        private async Task CheckVersion(CancellationToken cancellationToken)
        {
            using var client = _httpClientFactory.CreateClient();

            var versionResponse = await client.GetAsync(_configuration[_uriConfiguration], cancellationToken).ConfigureAwait(false);
            var version = await versionResponse.Content.ReadAsStringAsync();

            IsVersionCorrect = version.Equals(VersionConstant.Current);
            IsVersionChecked = true;

            VersionChecked?.Invoke();
        }

        private void CheckVersion(object sender, ConnectivityChangedEventArgs e)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                return;
            }

            _ = CheckVersion(CancellationToken.None);
            Connectivity.ConnectivityChanged -= CheckVersion;
        }
    }
}
