using Bootstrapper.Interface;
using DrugCheckingCrawler.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DrugCheckingCrawler
{
    public class ResourceCrawlerBootstrapper : IBootstrapper
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IResourceCrawler, ResourceCrawler>();
            services.AddSingleton<Parser>();
        }
    }
}
