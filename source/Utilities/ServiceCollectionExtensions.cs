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

            static TImplementation ImplementationFactory(IServiceProvider x) => x.GetRequiredService<TImplementation>();

            services.AddHostedSingletonService<TInterface, TImplementation>(ImplementationFactory);
        }

        public static void AddHostedSingletonService<TInterface, TImplementation>(this IServiceCollection serviceCollection, Func<IServiceProvider, TImplementation> implementationFactory)
            where TInterface : class
            where TImplementation : class, TInterface, IHostedService
        {
            serviceCollection.AddSingleton<TInterface, TImplementation>(implementationFactory);
            serviceCollection.AddHostedService(implementationFactory);
        }
    }
}
