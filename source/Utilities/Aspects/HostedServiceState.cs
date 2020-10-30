using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Utilities.Aspects
{
    public class HostedServiceState<T> : IHostedServiceState
    {
        private readonly IHostedService _decorated;
        private readonly ILogger _logger;

        public HostedServiceState(IHostedService decorated, ILogger logger)
        {
            _decorated = decorated;
            _logger = logger;
        }

        public bool IsStarted { get; private set; }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Starting {GetName()}... ");

            var result = _decorated.StartAsync(cancellationToken);

            _logger.LogInformation($"{GetName()} started");
            IsStarted = true;

            return result;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            IsStarted = false;
            _logger.LogInformation($"Stopping {GetName()}... ");

            var result = _decorated.StopAsync(cancellationToken);

            _logger.LogInformation($"{GetName()} stopped");

            return result;
        }

        private static string GetName() => typeof(T).GetFriendlyName();
    }
}
