using DatabaseInteraction.Interface;
using DatabaseInteraction.Repository;
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

            services.AddHostedSingletonService<ContainerFactory>();
            var isCached = configuration.ReadBool(_cachedConfiguration);
            if (isCached)
            {
                services.AddSingleton<IRepositoryFactory, CachedRepositoryFactory>(x => new CachedRepositoryFactory(x.GetService<ContainerFactory>().Container));
            }
            else
            {
                services.AddSingleton<IRepositoryFactory, RepositoryFactory>(x => new RepositoryFactory(x.GetService<ContainerFactory>().Container));
            }

            services.AddSingleton(x => x.GetService<IRepositoryFactory>().Create<CrawlerAction>());
            services.AddSingleton(x => x.GetService<IRepositoryFactory>().CreateDrugCheckingSourceRepository());

            services.AddSingleton<IEntityFactory, EntityFactory>();
        }
    }
}
