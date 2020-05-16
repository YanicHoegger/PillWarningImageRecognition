using Bootstrapper.Interface;
using Domain.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Utilities;

namespace Domain.Mock
{
    public class DomainMockBootstrapper : IBootstrapper
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHostedSingletonService<IPredicition, PredictionMock>();
        }
    }
}
