using DrugCheckingCrawler.Interface;
using DrugCheckingCrawler.Parsers;
using Microsoft.Extensions.DependencyInjection;

namespace DrugCheckingCrawler
{
    public static class ResourceCrawlerBootstrapper
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IResourceCrawler, ResourceCrawler>();
            services.AddSingleton<Parser>();
        }
    }
}
