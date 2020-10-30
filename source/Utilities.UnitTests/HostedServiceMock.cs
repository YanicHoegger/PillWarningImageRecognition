using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Utilities.UnitTests
{
    public interface IServiceMock
    {
        object Property { get; set; }
        void Method();
    }

    public class HostedServiceMock : IHostedService, IServiceMock
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public object Property { get; set; }

        public void Method()
        {
            //Nothing to do here
        }
    }
}
