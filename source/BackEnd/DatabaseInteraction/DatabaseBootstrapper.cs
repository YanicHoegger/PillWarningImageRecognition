using DatabaseInteraction.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Utilities;

namespace DatabaseInteraction
{
    public static class DatabaseBootstrapper
    {
        private const string _cachedConfiguration = "CACHED_DATABASE";
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IContext, ConfiguratedContext>();

            var isCached = configuration.ReadBool(_cachedConfiguration);
            if (isCached)
            {
                services.AddHostedSingletonService<IRepositoryFactory, CachedRepositoryFactory>();
            }
            else
            {
                services.AddHostedSingletonService<IRepositoryFactory, RepositoryFactory>();
            }

            services.AddSingleton(x => x.GetService<IRepositoryFactory>().Create<CrawlerAction>());
            services.AddSingleton(x => x.GetService<IRepositoryFactory>().Create<DrugCheckingSource>());

            services.AddSingleton<IEntityFactory, EntityFactory>();
            services.AddSingleton<IDataBaseUpdater, DataBaseUpdater>();
        }
    }
}
