using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DatabaseInteraction.Entity;
using DatabaseInteraction.Interface;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Hosting;

namespace DatabaseInteraction.Repository
{
    /// <summary>
    /// Use this class when for a long running usage of the implementation. 
    /// The first call will be very slow but all others will be fast. 
    /// If you use a short living application use <see cref="Repository{TInterface, TImplementation}"/>
    /// </summary>
    public class CachedRepository<TInterface, TImplementation> : RepositoryBase<TInterface, TImplementation>, IHostedService
        where TInterface : IEntity 
        where TImplementation : Entity.Entity, TInterface
    {
        private List<TImplementation> _cache;

        public CachedRepository(ContainerFactory<TInterface> containerFactory, EntityFactory entityFactory) 
            : base(containerFactory, entityFactory)
        {
        }

        public override Task Insert(TInterface entity)
        {
            _cache.Add(CreateEntity(entity));
            return base.Insert(entity);
        }

        public override Task Update(TInterface toUpdate, Guid id)
        {
            var toReplace = _cache.Single(x => x.Id == id);
            var index = _cache.IndexOf(toReplace);
            _cache[index] = CreateEntity(toUpdate);

            return base.Update(toUpdate, id);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _cache = await RetrieveList().ToListAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        protected override IAsyncEnumerable<TImplementation> GetInternal()
        {
            return _cache.ToAsyncEnumerable();
        }

        protected override IAsyncEnumerable<TImplementation> GetInternal(Func<IQueryable<TImplementation>, IQueryable<TImplementation>> queries)
        {
            return queries(_cache.AsQueryable()).ToAsyncEnumerable();
        }
    }
}
