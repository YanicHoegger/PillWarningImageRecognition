using Bootstrapper.Interface;
using DatabaseInteraction.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Utilities;

namespace DatabaseInteraction
{
    public class DatabaseBootstrapper : IBootstrapper
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IContext, ConfiguratedContext>();
            services.AddHostedSingletonService<IRepositoryFactory, RepositoryFactory>();

            services.AddSingleton<IEntityFactory, EntityFactory>();
        }
    }
}
