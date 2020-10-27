using System;
using System.Threading;
using System.Threading.Tasks;
using DatabaseInteraction.Interface;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DatabaseInteraction.Repository
{
    public class ContainerFactory : IHostedService
    {
        private readonly IContext _context;
        private readonly ILogger<ContainerFactory> _logger;

        private bool _isStarted;
        private Container _container;

        public ContainerFactory(IContext context, ILogger<ContainerFactory> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Starting {nameof(ContainerFactory)}");

            var client = ClientFactory.Create(_context);
            var databaseResponse = await client.CreateDatabaseIfNotExistsAsync(_context.DatabaseName, _context.Throughput, cancellationToken: cancellationToken);

            var containerResponse = await databaseResponse.Database.CreateContainerIfNotExistsAsync(_context.ContainerId, $"/{nameof(Entity.Id)}", cancellationToken: cancellationToken);
            Container = containerResponse.Container;

            _logger.LogInformation($"{nameof(ContainerFactory)} started");

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
