using Microsoft.Extensions.Hosting;

namespace Utilities.Aspects
{
    public interface IHostedServiceState : IHostedService
    {
        bool IsStarted { get; }
    }
}
