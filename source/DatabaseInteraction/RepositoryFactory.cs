using DatabaseInteraction.Interface;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DatabaseInteraction
{
    public class RepositoryFactory : IHostedService, IRepositoryFactory
    {
        private const int MaxThroughput = 400;

        private readonly ILogger<RepositoryFactory> _logger;
        private readonly IContext _context;

        private bool _isStarted;
        private Container _container;

        public RepositoryFactory(IContext context, ILogger<RepositoryFactory> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IRepository<T> Create<T>() where T : Entity, new()
        {
            if (!_isStarted)
                throw new InvalidOperationException($"{nameof(RepositoryFactory)} must be started first");

            return new Repository<T>(_container);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var client = ClientFactory.Create(_context);
            var databaseResponse = await client.CreateDatabaseIfNotExistsAsync(_context.DatabaseName, MaxThroughput);

            var containerResponse = await databaseResponse.Database.CreateContainerIfNotExistsAsync(_context.ContainerId, $"/{nameof(Entity.Id)}");
            _container = containerResponse.Container;

            _isStarted = true;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
