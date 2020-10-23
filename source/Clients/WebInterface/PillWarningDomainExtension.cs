using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebInterface
{
    public static class PillWarningDomainExtension
    {
        public static void AddDomain(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            Bootstrapper.Bootstrapper.ConfigureServicesForPillPrediction(serviceCollection, configuration);
        }
    }
}
