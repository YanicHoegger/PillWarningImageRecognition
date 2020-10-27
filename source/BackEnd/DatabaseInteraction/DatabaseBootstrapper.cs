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

            var isCached = configuration.ReadBool(_cachedConfiguration);
            if (isCached)
            {
                services.AddRepository<CrawlerAction, IRepository<CrawlerAction>, CachedRepository<CrawlerAction>>();
                services.AddRepository<DrugCheckingSource, IDrugCheckingSourceRepository, CachedDrugCheckingSourceRepository>();
            }
            else
            {
                services.AddRepository<CrawlerAction, IRepository<CrawlerAction>, Repository<CrawlerAction>>();
                services.AddRepository<DrugCheckingSource, IDrugCheckingSourceRepository, DrugCheckingSourceRepository>();
            }

            services.AddSingleton<IEntityFactory, EntityFactory>();
        }

        private static void AddRepository<TEntity, TRepositoryInterface, TRepositoryImplementation>(this IServiceCollection services)
            where TEntity : Entity, new()
            where TRepositoryInterface : class, IRepository<TEntity>
            where TRepositoryImplementation : class, TRepositoryInterface
        {
            services.AddHostedSingletonService<ContainerFactory<TEntity>>();
            services.AddSingleton<TRepositoryInterface, TRepositoryImplementation>();
        }
    }
}
