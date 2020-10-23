using DatabaseInteraction;
using Domain;
using ImageInteraction;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Utilities;

namespace WebInterface
{
    public static class PillWarningDomainExtension
    {
        public static void AddDomain(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
#if DEBUG

            if (configuration.ReadBool("MOCK"))
            {
                Domain.Mock.DomainMockBootstrapper.ConfigureServices(serviceCollection);
                return;
            }

#endif
            DomainBootstrapper.ConfigureServicesForPrediction(serviceCollection);
            DatabaseBootstrapper.ConfigureServices(serviceCollection, configuration);
            ImageInteractionBootstrapper.ConfigureServicesForPillRecognition(serviceCollection);
        }
    }
}
