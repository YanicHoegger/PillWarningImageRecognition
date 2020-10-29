using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseInteraction.Entity;
using DatabaseInteraction.Interface;
using Microsoft.Azure.Cosmos;

namespace DatabaseInteraction.Repository
{
    public abstract class RepositoryBase<TInterface, TImplementation> : IRepository<TInterface> 
        where TInterface : IEntity
        where TImplementation : Entity.Entity, TInterface
    {
        private readonly EntityFactory _entityFactory;

        protected RepositoryBase(ContainerFactory<TInterface> containerFactory, EntityFactory entityFactory)
        {
            _entityFactory = entityFactory;
            Container = containerFactory.Container;
        }

        public IAsyncEnumerable<TInterface> Get()
        {
            return GetInternal();
        }

        public virtual async Task Insert(TInterface entity)
        {
            var implementation = CreateEntity(entity);

            await Container.CreateItemAsync(implementation);
        }

        public virtual async Task Update(TInterface toUpdate, Guid id)
        {
            var implementation = CreateEntity(toUpdate);
            implementation.Id = id;

            await Container.ReplaceItemAsync(toUpdate, id.ToString());
        }

        protected Container Container { get; }

        protected abstract IAsyncEnumerable<TImplementation> GetInternal();
        protected abstract IAsyncEnumerable<TImplementation> GetInternal(Func<IQueryable<TImplementation>, IQueryable<TImplementation>> queries);

        protected IOrderedQueryable<TImplementation> GetFeedIterator()
        {
            return Container.GetItemLinqQueryable<TImplementation>(true);
        }

        protected async IAsyncEnumerable<TImplementation> RetrieveList(FeedIterator<TImplementation> feedIterator)
        {
            while (feedIterator.HasMoreResults)
            {
                foreach (var result in await feedIterator.ReadNextAsync())
                {
                    yield return result;
                }
            }
        }

        protected TImplementation CreateEntity(TInterface entity)
        {
            return _entityFactory.CreateImplementation<TInterface, TImplementation>(entity);
        }
    }
}
