using DatabaseInteraction;
using Domain;
using DrugCheckingCrawler;
using ImageInteraction;
using Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ManipulationClient
{
    public static class ServiceConfigurator
    {
        public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            DomainBootstrapper.ConfigureServiceForManipulation(services);
            DatabaseBootstrapper.ConfigureServices(services, configuration);
            ImageInteractionBootstrapper.ConfigureServicesForManipulation(services);
            ResourceCrawlerBootstrapper.ConfigureServices(services);

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
