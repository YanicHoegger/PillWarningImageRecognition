using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseInteraction
{
    public class Repository<T> where T : IEntity
    {
        private readonly Container _container;

        public Repository(Container container)
        {
            _container = container;
        }

        public async Task<List<T>> Get()
        {
            var queryDefinition = QueryResolver.SelectOfType<T>();
            var feedIterator = _container.GetItemQueryIterator<T>(queryDefinition);

            return await RetrieveList(feedIterator);
        }

        //TODO: Find a way to creat the query out of linq
        public async Task<List<T>> Find(string queryDefinition)
        {
            var feedIterator = _container.GetItemQueryIterator<T>(queryDefinition);
            return await RetrieveList(feedIterator);
        }

        private async Task<List<T>> RetrieveList(FeedIterator<T> feedIterator)
        {
            var resultList = new List<T>();

            while (feedIterator.HasMoreResults)
            {
                var response = await feedIterator.ReadNextAsync();
                resultList.AddRange(response.ToList());
            }

            return resultList;
        }

        public async Task Insert(T entity)
        {
            entity.Id = Guid.NewGuid();

            await _container.CreateItemAsync(entity);
        }

        public async Task Insert(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                await Insert(entity);
            }
        }

        public async Task DeleteContainer()
        {
            await _container.DeleteContainerAsync();
        }
    }
}
