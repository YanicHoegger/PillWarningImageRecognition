using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseInteraction.Entity;
using DatabaseInteraction.Interface;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace DatabaseInteraction.Repository
{
    public abstract class RepositoryBase<TInterface, TImplementation> : IRepository<TInterface> 
        where TInterface : IEntity
        where TImplementation : Entity.Entity, TInterface
    {
        private readonly EntityFactory _entityFactory;
        private readonly Func<Container> _containerFunc;

        protected RepositoryBase(IContainerFactory<TInterface> containerFactory, EntityFactory entityFactory)
        {
            _entityFactory = entityFactory;
            _containerFunc = () => containerFactory.Container;
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

            await Container.ReplaceItemAsync(implementation, id.ToString());
        }

        protected Container Container => _containerFunc();

        protected abstract IAsyncEnumerable<TImplementation> GetInternal();
        // ReSharper disable once UnusedMemberInSuper.Global
        protected abstract IAsyncEnumerable<TImplementation> GetInternal(Func<IQueryable<TImplementation>, IQueryable<TImplementation>> queries);

        protected IAsyncEnumerable<TImplementation> RetrieveList()
        {
            return RetrieveList(entities => entities);
        }

        protected async IAsyncEnumerable<TImplementation> RetrieveList(Func<IQueryable<TImplementation>, IQueryable<TImplementation>> queries)
        {
            var feedIterator = queries(Container.GetItemLinqQueryable<TImplementation>(true)).ToFeedIterator();
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
