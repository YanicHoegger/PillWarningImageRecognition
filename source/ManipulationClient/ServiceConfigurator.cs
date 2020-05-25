using CustomVisionInteraction;
using DatabaseInteraction;
using Domain;
using DrugCheckingCrawler;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ManipulationClient
{
    public static class ServiceConfigurator
    {
        public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            if(GetBoolValue(configuration, "CrawlDrugCheckingSource"))
            {
                ConfigureDrugCheckingSourceCrawler(configuration, services);
            }
        }

        private static void ConfigureDrugCheckingSourceCrawler(IConfiguration configuration, IServiceCollection services)
        {
            //TODO: this logic belongs into the bootstrapper
            new DatabaseBootstrapper().ConfigureServices(services, configuration);
            new CustomVisionBootstrapper().ConfigureServices(services, configuration);
            new DrugCheckingSourceBootstrapper().ConfigureServices(services, configuration);
            new ResourceCrawlerBootstrapper().ConfigureServices(services, configuration);

            //services.AddScoped<IExecuter, DrugCheckingCrawler>();
            services.AddScoped<IExecuter, DrugCheckingUpdater>();
        }

        private static bool GetBoolValue(IConfiguration configuration, string configName)
        {
            var hasConversation = bool.TryParse(configuration[configName], out var parsed);
            return hasConversation && parsed;
        }
    }
}
