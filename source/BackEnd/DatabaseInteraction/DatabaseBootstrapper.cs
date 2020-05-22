using Bootstrapper.Interface;
using DatabaseInteraction.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Utilities;

namespace DatabaseInteraction
{
    public class DatabaseBootstrapper : IBootstrapper
    {
        private const string _cachedConfiguration = "CACHED_DATABASE";
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IContext, ConfiguratedContext>();

            var couldParse = bool.TryParse(configuration[_cachedConfiguration], out var isCached);
            if (couldParse && isCached)
            {
                services.AddHostedSingletonService<IRepositoryFactory, CachedRepositoryFactory>();
            }
            else
            {
                services.AddHostedSingletonService<IRepositoryFactory, RepositoryFactory>();
            }

            services.AddSingleton<IEntityFactory, EntityFactory>();
        }
    }
}
