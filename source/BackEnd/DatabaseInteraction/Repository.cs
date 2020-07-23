using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Threading.Tasks;
using DatabaseInteraction.Interface;

namespace DatabaseInteraction
{
    public class Repository<T> : RepositoryBase<T> where T : Entity, new()
    {
        public Repository(Container container) 
            : base(container)
        {
        }

        //TODO: Find a way to creat the query out of linq
        public async Task<List<T>> Find(string queryDefinition)
        {
            var feedIterator = Container.GetItemQueryIterator<T>(queryDefinition);
            return await RetrieveList(feedIterator);
        }

        public override Task<List<T>> Get()
        {
            return GetFromDb();
        }
    }
}
