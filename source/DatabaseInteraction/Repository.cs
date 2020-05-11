using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseInteraction
{
    public class Repository<T> where T : IEntity
    {
        private readonly IMongoCollection<T> _collection;

        public Repository(IContext context)
        {
            var client = ClientFactory.Create(context);
            var database = client.GetDatabase(context.DatabaseName);

            _collection = database.GetCollection<T>(typeof(T).ToString());
        }

        public async Task<List<T>> Get()
        {
            return (await _collection.FindAsync(x => true)).ToList();
        }

        public async Task Insert(T entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task Insert(IEnumerable<T> entitys)
        {
            await _collection.InsertManyAsync(entitys);
        }

        public async Task DeleteAll()
        {
            await _collection.DeleteManyAsync(x => true);
        }
    }
}
