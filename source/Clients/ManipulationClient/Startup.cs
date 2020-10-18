using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ManipulationClient
{
    public class Startup
    {
        public static IServiceProvider ServiceProvider { get; set; }

        public static void Init(string[] args)
        {
            var host = Host
                .CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureAppConfiguration((hostContext, builder) =>
                {
                    builder.AddUserSecrets<Program>();
                })
                .ConfigureServices((hostBuilderContext, serviceCollection) =>
                {
                    ServiceConfigurator.ConfigureServices(hostBuilderContext.Configuration, serviceCollection);
                })
                .Build();

            ServiceProvider = host.Services;

            Task.WaitAll(ServiceProvider
                .GetServices<IHostedService>()
                .Select(service => service.StartAsync(CancellationToken.None))
                .ToArray());
        }
    }
}
