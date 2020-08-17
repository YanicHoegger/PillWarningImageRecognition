using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MobileInterface.Services;
using MobileInterface.Services.Mock;
using MobileInterface.ViewModels;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MobileInterface
{
    public static class Startup
    {
        private static CancellationTokenSource _cancellationTokenSource;

        public static IServiceProvider ServiceProvider { get; set; }

        public static void Init()
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            using var stream = executingAssembly.GetManifestResourceStream("MobileInterface.appsettings.json");

            var host = new HostBuilder()
                        .ConfigureHostConfiguration(configurationBuilder =>
                        {
                            configurationBuilder.AddCommandLine(new[] { $"ContentRoot={FileSystem.AppDataDirectory}" });
                            configurationBuilder.AddJsonStream(stream);
                        })
                        .ConfigureServices(ConfigureServices)
                        //TODO: Use different logging
                        .ConfigureLogging(loggingBuilder => loggingBuilder.AddConsole(options =>
                        {
                            options.DisableColors = true;
                        }))
                        .Build();

            ServiceProvider = host.Services;
        }

        public static void Start()
        {
            StartInternal();
        }

        public static void Stop()
        {
            var tasks = ServiceProvider
                .GetServices<IHostedService>()
                .Select(service => service.StopAsync(_cancellationTokenSource.Token))
                .ToList();

            Task.WhenAny(Task.WhenAll(tasks), Task.Delay(1000));
            _cancellationTokenSource?.Cancel();
        }

        public static void Resume()
        {
            StartInternal();
        }

        static void StartInternal()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            foreach (var service in ServiceProvider.GetServices<IHostedService>())
            {
                service.StartAsync(_cancellationTokenSource.Token);
            }
        }


        static void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
        {
#if DEBUG
            if (ctx.HostingEnvironment.IsDevelopment())
            {
                services.AddSingleton<IPredictionService, PredictionServiceMock>();
            }
            else
            {
                services.AddSingleton<IPredictionService, PredictionService>();
            }
#else
            services.AddSingleton<IPredictionService, PredictionService>();
#endif

            services.AddTransient<PreditionViewModel>();
            services.AddHostedService<MediaPluginInitService>();

            services.AddHttpClient();
        }
    }
}
