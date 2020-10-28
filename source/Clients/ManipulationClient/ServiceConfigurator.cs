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
            ImageInteractionBootstrapper.ConfigureServicesForManipulation(services, configuration);
            ResourceCrawlerBootstrapper.ConfigureServices(services);

            //TODO: Make Dictionary out of this (or mapping or something)
            if (configuration.ReadBool("Update"))
            {
                services.AddSingleton<IExecuter, DrugCheckingUpdater>();
            }
            if (configuration.ReadBool("Crawl"))
            {
                services.AddSingleton<IExecuter, DrugCheckingCrawler>();
            }
            if (configuration.ReadBool("CleanPrediction"))
            {
                services.AddSingleton<IExecuter, PredictedImagesCleaner>();
            }
        }
    }
}
