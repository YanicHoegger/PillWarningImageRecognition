using System.Net.Http;
using System.Threading.Tasks;
using Clients.Shared;
using Microsoft.Extensions.Configuration;
using Utilities;

namespace MobileInterface.Services
{
    public class VersionCheckerService : IVersionCheckerService
    {
        private const string _uriConfiguration = "VersionUri";

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public VersionCheckerService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<bool> GetIsCorrectServerVersion()
        {
            var client = _httpClientFactory.CreateClient();

            var temp = await client.GetAsync(_configuration[_uriConfiguration]);

            var version = await client.GetAsync<string>(_configuration[_uriConfiguration]);

            return version.Equals(VersionConstant.Current);
        }
    }
}
