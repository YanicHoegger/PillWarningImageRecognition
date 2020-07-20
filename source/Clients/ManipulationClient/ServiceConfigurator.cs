using CustomVisionInteraction;
using DatabaseInteraction;
using Domain;
using DrugCheckingCrawler;
using Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ManipulationClient
{
    public static class ServiceConfigurator
    {
        public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            //TODO: this logic belongs into the bootstrapper --> Then also the project references can get removed
            new DatabaseBootstrapper().ConfigureServices(services, configuration);
            new CustomVisionBootstrapper().ConfigureServices(services, configuration);
            new DrugCheckingSourceBootstrapper().ConfigureServices(services, configuration);
            new ResourceCrawlerBootstrapper().ConfigureServices(services, configuration);

            if (configuration.ReadBool("Update"))
            {
                services.AddScoped<IExecuter, DrugCheckingUpdater>();
            }
            if (configuration.ReadBool("Crawl"))
            {
                services.AddScoped<IExecuter, DrugCheckingCrawler>();
            }
        }
    }
}
