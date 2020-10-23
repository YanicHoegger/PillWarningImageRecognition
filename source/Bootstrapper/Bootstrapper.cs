using DatabaseInteraction;
using Domain;
using DrugCheckingCrawler;
using Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ImageInteraction;

namespace Bootstrapper
{
    public class Bootstrapper
    {
        public static void ConfigureServicesForPillPrediction(IServiceCollection services, IConfiguration configuration)
        {
#if DEBUG

            if (configuration.ReadBool("MOCK"))
            {
                Domain.Mock.DomainMockBootstrapper.ConfigureServices(services);
                return;
            }

#endif
            DomainBootstrapper.ConfigureServicesForPrediction(services);
            DatabaseBootstrapper.ConfigureServices(services, configuration);
            CustomVisionBootstrapper.ConfigureServicesForPillRecognition(services);
        }

        public static void ConfigureServicesForManipulation(IServiceCollection services, IConfiguration configuration)
        {
            DomainBootstrapper.ConfigureServiceForManipulation(services);
            DatabaseBootstrapper.ConfigureServices(services, configuration);
            CustomVisionBootstrapper.ConfigureServicesForManipulation(services);
            ResourceCrawlerBootstrapper.ConfigureServices(services);
        }
    }
}
