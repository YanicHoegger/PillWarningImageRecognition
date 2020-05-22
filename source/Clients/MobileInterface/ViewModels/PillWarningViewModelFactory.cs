using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Threading.Tasks;

namespace MobileInterface.ViewModels
{
    public static class PillWarningViewModelFactory
    {
        public static async Task<PillWarningViewModel> Create(string uri)
        {
            var httpClientFactory = Startup.ServiceProvider.GetService<IHttpClientFactory>();
            var client = httpClientFactory.CreateClient();

            return new PillWarningViewModel(await client.GetStreamAsync(uri));
        }
    }
}
