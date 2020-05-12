using System.Threading.Tasks;

namespace DatabaseInteraction
{
    public static class RepositoryFactory
    {
        private const int MaxThroughput = 400;

        public static async Task<Repository<T>> Create<T>(IContext context) where T : IEntity
        {
            var client = ClientFactory.Create(context);
            var databaseResponse = await client.CreateDatabaseIfNotExistsAsync(context.DatabaseName, MaxThroughput);

            var containerResponse = await databaseResponse.Database.CreateContainerIfNotExistsAsync(context.ContainerId, $"/{nameof(IEntity.Id)}");

            return new Repository<T>(containerResponse.Container);
        }
    }
}
