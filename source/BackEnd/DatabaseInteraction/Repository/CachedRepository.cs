using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseInteraction.Interface;
using Microsoft.Azure.Cosmos;

namespace DatabaseInteraction.Repository
{
    /// <summary>
    /// Use this class when for a long running usage of the implementation. 
    /// The first call will be very slow but all others will be fast. 
    /// If you use a short living application use <see cref="Repository{T}"/>
    /// </summary>
    public class CachedRepository<T> : RepositoryBase<T> 
        where T : Entity, new()
    {
        private List<T> _cache;

        public CachedRepository(Container container) 
            : base(container)
        {
        }

        public override async IAsyncEnumerable<T> Get()
        {
            await CheckCache();

            foreach (var entity in _cache)
            {
                yield return entity;
            }
        }

        public override async IAsyncEnumerable<T> Get(Func<IQueryable<T>, IQueryable<T>> queries)
        {
            await CheckCache();

            foreach (var entity in queries(_cache.AsQueryable()))
            {
                yield return entity;
            }
        }

        public override Task Insert(T entity)
        {
            _cache.Add(entity);
            return base.Insert(entity);
        }

        public override Task Update(T toUpdate, Guid id)
        {
            var toReplace = _cache.Single(x => x.Id == id);
            var index = _cache.IndexOf(toReplace);
            _cache[index] = toUpdate;

            return base.Update(toUpdate, id);
        }

        protected IAsyncEnumerable<T> GetFromDb()
        {
            var queryDefinition = QueryResolver.SelectOfType<T>();
            var feedIterator = Container.GetItemQueryIterator<T>(queryDefinition);

            return RetrieveList(feedIterator);
        }

        private async Task CheckCache()
        {
            _cache ??= await GetFromDb().ToListAsync();
        }
    }
}
