using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseInteraction.Interface;
using Microsoft.Azure.Cosmos;

namespace DatabaseInteraction.Repository
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : Entity, new()
    {
        protected RepositoryBase(Container container)
        {
            Container = container;
        }

        public abstract IAsyncEnumerable<T> Get();
        public abstract IAsyncEnumerable<T> Get(Func<IQueryable<T>, IQueryable<T>> queries);

        public virtual async Task Insert(T entity)
        {
            await Container.CreateItemAsync(entity);
        }

        public virtual async Task Update(T toUpdate, Guid id)
        {
            toUpdate.Id = id;
            await Container.ReplaceItemAsync(toUpdate, id.ToString());
        }

        protected Container Container { get; }

        protected IOrderedQueryable<T> GetFeedIterator()
        {
            return Container.GetItemLinqQueryable<T>(true);
        }

        protected async IAsyncEnumerable<T> RetrieveList(FeedIterator<T> feedIterator)
        {
            while (feedIterator.HasMoreResults)
            {
                foreach (var result in await feedIterator.ReadNextAsync())
                {
                    yield return result;
                }
            }
        }
    }
}
