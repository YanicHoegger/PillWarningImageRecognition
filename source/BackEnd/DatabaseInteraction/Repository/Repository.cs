using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseInteraction.Interface;
using Microsoft.Azure.Cosmos.Linq;

namespace DatabaseInteraction.Repository
{
    public class Repository<T> : RepositoryBase<T> where T : Entity, new()
    {
        public Repository(ContainerFactory<T> containerFactory) 
            : base(containerFactory)
        {
        }

        public override IAsyncEnumerable<T> Get()
        {
            return RetrieveList(GetFeedIterator().ToFeedIterator());
        }

        public override IAsyncEnumerable<T> Get(Func<IQueryable<T>, IQueryable<T>> queries)
        {
            var feedIterator = queries(GetFeedIterator()).ToFeedIterator();
            return RetrieveList(feedIterator);
        }
    }
}
