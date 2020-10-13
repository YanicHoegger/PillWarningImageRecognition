using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using MobileInterface.Services;
#if DEBUG
using MobileInterface.Services.Mock;
#endif
using MobileInterface.ViewModels;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serilog.Extensions.Logging;
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
                            // ReSharper disable once AccessToDisposedClosure : Is ok since we use it before leaving the scope
                            configurationBuilder.AddJsonStream(stream);
                        })
                        .ConfigureServices(ConfigureServices)
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

            services.AddSingleton<IVersionCheckerService, VersionCheckerService>();
            services.AddTransient<PredictionViewModel>();
            services.AddHostedService<MediaPluginInitService>();

            services.AddHttpClient();

            ConfigureLogging(services);
        }

        private static void ConfigureLogging(IServiceCollection services)
        {
            var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var path = Path.Combine(basePath, "log.txt");

            var logger = new LoggerConfiguration()
                .WriteTo.File(path, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 1,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] ({SourceContext}) {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            services.AddSingleton<ILoggerFactory>(_ => new SerilogLoggerFactory(logger));
        }
    }
}
