using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Utilities
{
    public static class ServiceCollectionExtensions
    {
        public static void AddHostedSingletonService<TInterface, TImplementation>(this IServiceCollection services)
            where TInterface : class
            where TImplementation : class, TInterface, IHostedService
        {
            services.AddSingleton<TImplementation>();

            static TImplementation implementationFactory(IServiceProvider x) => x.GetRequiredService<TImplementation>();

            services.AddSingleton<TInterface, TImplementation>(implementationFactory);
            services.AddHostedService(implementationFactory);
        }
    }
}
