using DatabaseInteraction.Entity;
using DatabaseInteraction.Interface;
using DatabaseInteraction.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
                services.AddHostedRepository<ICrawlerAction, IRepository<ICrawlerAction>, CachedRepository<ICrawlerAction, CrawlerAction>>();
                services.AddHostedRepository<IDrugCheckingSource, IDrugCheckingSourceRepository, CachedDrugCheckingSourceRepository>();
            }
            else
            {
                services.AddRepository<ICrawlerAction, IRepository<ICrawlerAction>, Repository<ICrawlerAction, CrawlerAction>>();
                services.AddRepository<IDrugCheckingSource, IDrugCheckingSourceRepository, DrugCheckingSourceRepository>();
            }

            services.AddEntity<ICrawlerAction, CrawlerAction>();
            services.AddEntity<IDrugCheckingSource, DrugCheckingSource>();
            services.AddEntity<IDrugCheckingInfo, DrugCheckingInfo>();

            services.AddSingleton<EntityFactory>();
            services.AddSingleton<IEntityFactory, EntityFactory>(serviceProvider => serviceProvider.GetService<EntityFactory>());

            services.AddSingleton<ClientFactory>();
            services.AddSingleton<JsonCosmosSerializer>();
        }

        private static void AddRepository<TEntity, TRepositoryInterface, TRepositoryImplementation>(this IServiceCollection services)
            where TEntity : IEntity
            where TRepositoryInterface : class, IRepository<TEntity>
            where TRepositoryImplementation : class, TRepositoryInterface
        {
            services.AddHostedSingletonService<IContainerFactory<TEntity>, ContainerFactory<TEntity>>();
            services.AddSingleton<TRepositoryInterface, TRepositoryImplementation>();
        }

        private static void AddHostedRepository<TEntity, TRepositoryInterface, TRepositoryImplementation>(this IServiceCollection services)
            where TEntity : IEntity
            where TRepositoryInterface : class, IRepository<TEntity>
            where TRepositoryImplementation : class, TRepositoryInterface, IHostedService
        {
            services.AddHostedSingletonService<IContainerFactory<TEntity>, ContainerFactory<TEntity>>();
            services.AddHostedSingletonService<TRepositoryInterface, TRepositoryImplementation>();
        }

        private static void AddEntity<TInterface, TImplementation>(this IServiceCollection services)
            where TInterface : IEntity
            where TImplementation : Entity.Entity, TInterface
        {
            services.AddSingleton<EntityMapping, EntityMapping.TypedEntityMapping<TInterface, TImplementation>>();
        }
    }
}
