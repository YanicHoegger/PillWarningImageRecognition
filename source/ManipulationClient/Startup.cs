using DatabaseInteraction;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;

namespace ManipulationClient
{
    public class Startup
    {
        public static IServiceProvider ServiceProvider { get; set; }

        public static void Init(string[] args)
        {
            var host = Host
                .CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostContext, builder) =>
                {
                    builder.AddUserSecrets<Program>();
                })
                .ConfigureServices((hostBuilderContext, serviceCollection) =>
                {
                    ConfigureServices(hostBuilderContext, serviceCollection);
                })
                .Build();

            ServiceProvider = host.Services;

            var cancellationTokenSource = new CancellationTokenSource();
            foreach (var service in ServiceProvider.GetServices<IHostedService>())
            {
                service.StartAsync(cancellationTokenSource.Token).Wait();
            }
        }

        private static void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
        {
            new DatabaseBootstrapper().ConfigureServices(services, ctx.Configuration);
        }
    }
}
