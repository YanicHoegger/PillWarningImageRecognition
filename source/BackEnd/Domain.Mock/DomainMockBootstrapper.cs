using Domain.Interface;
using Microsoft.Extensions.DependencyInjection;
using Utilities;

namespace Domain.Mock
{
    public class DomainMockBootstrapper
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedSingletonService<IPredicition, PredictionMock>();
        }
    }
}
