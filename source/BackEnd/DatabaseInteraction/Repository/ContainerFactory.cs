using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Hosting;

namespace DatabaseInteraction.Repository
{
    public class ContainerFactory<T> : IHostedService, IContainerFactory<T>
    {
        private readonly IContext _context;
        private readonly ClientFactory _clientFactory;

        public ContainerFactory(IContext context, ClientFactory clientFactory)
        {
            _context = context;
            _clientFactory = clientFactory;
        }

        public Container Container { get; private set; }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var client = _clientFactory.Create(_context);
            var databaseResponse = await client.CreateDatabaseIfNotExistsAsync(_context.DatabaseName, _context.Throughput, cancellationToken: cancellationToken);

            var containerResponse = await databaseResponse.Database.CreateContainerIfNotExistsAsync(typeof(T).Name, $"/{nameof(Entity.Entity.Id)}", cancellationToken: cancellationToken);
            Container = containerResponse.Container;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
