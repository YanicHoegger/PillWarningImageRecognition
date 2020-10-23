using Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ManipulationClient
{
    public static class ServiceConfigurator
    {
        public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            Bootstrapper.Bootstrapper.ConfigureServicesForManipulation(services, configuration);

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
