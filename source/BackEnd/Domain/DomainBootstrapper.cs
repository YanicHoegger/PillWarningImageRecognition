using Bootstrapper.Interface;
using Domain.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Domain
{
    public class DomainBootstrapper : IBootstrapper
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IPredicition, Predicition>();
            services.AddSingleton<IPillColorAnalyzer, PillColorAnalyzer>();
        }
    }
}
