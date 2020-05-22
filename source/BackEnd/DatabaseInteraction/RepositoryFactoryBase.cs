using DatabaseInteraction.Interface;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DatabaseInteraction
{
    public abstract class RepositoryFactoryBase : IHostedService, IRepositoryFactory
    {
        private const int _maxThroughput = 400;

        private readonly IContext _context;
        private readonly ILogger<RepositoryFactoryBase> _logger;

        private bool _isStarted;

        protected RepositoryFactoryBase(IContext context, ILogger<RepositoryFactoryBase> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IRepository<T> Create<T>() where T : Entity, new()
        {
            if (!_isStarted)
                throw new InvalidOperationException($"{nameof(RepositoryFactory)} must be started first");

            return OnCreate<T>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var client = ClientFactory.Create(_context);
            var databaseResponse = await client.CreateDatabaseIfNotExistsAsync(_context.DatabaseName, _maxThroughput);

            var containerResponse = await databaseResponse.Database.CreateContainerIfNotExistsAsync(_context.ContainerId, $"/{nameof(Entity.Id)}");
            Container = containerResponse.Container;

            _isStarted = true;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        protected abstract IRepository<T> OnCreate<T>() where T : Entity, new();

        protected Container Container { get; private set; }
    }
}
