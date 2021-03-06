﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
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

            var cancellationTokenSource = new CancellationTokenSource();
            foreach (var service in ServiceProvider.GetServices<IHostedService>())
            {
                service.StartAsync(cancellationTokenSource.Token).Wait();
            }
        }
    }
}
