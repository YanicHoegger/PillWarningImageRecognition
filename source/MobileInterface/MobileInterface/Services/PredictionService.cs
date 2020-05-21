using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using WebInterface.Shared;

namespace MobileInterface.Services
{
    public class PredictionService : IPredictionService
    {
        private const string _uriConfiguration = "PillWarningUri";

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public PredictionService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<PredictionResult> Predict(Stream image, string name)
        {
            var client = _httpClientFactory.CreateClient();
            return await client.PostImageAsync<PredictionResult>(image, name, _configuration[_uriConfiguration]);
        }
    }
}
