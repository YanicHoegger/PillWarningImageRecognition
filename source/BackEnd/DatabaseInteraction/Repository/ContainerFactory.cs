using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DatabaseInteraction.Repository
{
    public class ContainerFactory<T> : IHostedService
    {
        private readonly IContext _context;
        private readonly ILogger<ContainerFactory<T>> _logger;
        private readonly ClientFactory _clientFactory;

        private bool _isStarted;
        private Container _container;

        public ContainerFactory(IContext context, ILogger<ContainerFactory<T>> logger, ClientFactory clientFactory)
        {
            _context = context;
            _logger = logger;
            _clientFactory = clientFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Starting {nameof(ContainerFactory<T>)}");

            var client = _clientFactory.Create(_context);
            var databaseResponse = await client.CreateDatabaseIfNotExistsAsync(_context.DatabaseName, _context.Throughput, cancellationToken: cancellationToken);

            var containerResponse = await databaseResponse.Database.CreateContainerIfNotExistsAsync(typeof(T).Name, $"/{nameof(Entity.Entity.Id)}", cancellationToken: cancellationToken);
            Container = containerResponse.Container;

            _logger.LogInformation($"{nameof(ContainerFactory<T>)} started");

            _isStarted = true;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Container Container
        {
            get
            {
                if(!_isStarted)
                    throw new InvalidOperationException("Service needs to get started first");
                return _container;
            }

            private set => _container = value;
        }
    }
}
