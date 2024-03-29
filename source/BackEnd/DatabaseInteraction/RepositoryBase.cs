﻿using DatabaseInteraction.Interface;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseInteraction
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : Entity, new()
    {
        public RepositoryBase(Container container)
        {
            Container = container;
        }

        public abstract Task<List<T>> Get();

        public async Task Insert(T entity)
        {
            await InsertInternal(entity);
            OnInsert();
        }

        public async Task Insert(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                await InsertInternal(entity);
            }
            OnInsert();
        }

        public async Task Update(T toUpdate, Guid id)
        {
            toUpdate.Id = id;
            await Container.ReplaceItemAsync(toUpdate, id.ToString());
        }

        protected Container Container { get; }

        protected async Task<List<T>> GetFromDb()
        {
            var queryDefinition = QueryResolver.SelectOfType<T>();
            var feedIterator = Container.GetItemQueryIterator<T>(queryDefinition);

            return await RetrieveList(feedIterator);
        }

        protected async Task<List<T>> RetrieveList(FeedIterator<T> feedIterator)
        {
            var resultList = new List<T>();

            while (feedIterator.HasMoreResults)
            {
                var response = await feedIterator.ReadNextAsync();
                resultList.AddRange(response.ToList());
            }

            return resultList;
        }

        protected async Task<List<TT>> RetrieveListTemp<TT>(FeedIterator<TT> feedIterator)
        {
            var resultList = new List<TT>();

            while (feedIterator.HasMoreResults)
            {
                var response = await feedIterator.ReadNextAsync();
                resultList.AddRange(response.ToList());
            }

            return resultList;
        }

        protected virtual void OnInsert() { }

        private async Task InsertInternal(T entity)
        {
            await Container.CreateItemAsync(entity);
        }
    }
}
