using System.Net.Http;
using System.Threading.Tasks;
using Clients.Shared;
using Microsoft.Extensions.Configuration;

namespace MobileInterface.Services
{
    public class VersionCheckerService : IVersionCheckerService
    {
        private const string _uriConfiguration = "VersionUri";

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        //The version would not change unless recompiled, so check it once is good enough
        private bool _hasAlreadyChecked;
        private bool _previewousResult;

        public VersionCheckerService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<bool> GetIsCorrectServerVersion()
        {
            if (!_hasAlreadyChecked)
            {
                using var client = _httpClientFactory.CreateClient();

                var versionResponse = await client.GetAsync(_configuration[_uriConfiguration]).ConfigureAwait(false);
                var version = await versionResponse.Content.ReadAsStringAsync();

                _previewousResult = version.Equals(VersionConstant.Current);
                _hasAlreadyChecked = true;
            }

            return _previewousResult;
        }
    }
}
