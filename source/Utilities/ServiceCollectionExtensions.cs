using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Utilities
{
    public static class ServiceCollectionExtensions
    {
        public static void AddHostedSingletonService<TInterface, TImplementation>(this IServiceCollection serviceCollection)
            where TInterface : class
            where TImplementation : class, TInterface, IHostedService
        {
            serviceCollection.AddSingleton<TImplementation>();

            serviceCollection.RegisterAsInterfaceAndHostedService<TInterface, TImplementation>();
        }

        public static void AddHostedSingletonService<TInterface, TImplementation>(this IServiceCollection serviceCollection, Func<IServiceProvider, TImplementation> implementationFactory)
            where TInterface : class
            where TImplementation : class, TInterface, IHostedService
        {
            serviceCollection.AddSingleton(implementationFactory);

            serviceCollection.RegisterAsInterfaceAndHostedService<TInterface, TImplementation>();
        }

        private static void RegisterAsInterfaceAndHostedService<TInterface, TImplementation>(this IServiceCollection serviceCollection)
            where TInterface : class where TImplementation : class, TInterface, IHostedService
        {
            static TImplementation ImplementationFactory(IServiceProvider x) => x.GetRequiredService<TImplementation>();

            serviceCollection.AddSingleton<TInterface, TImplementation>(ImplementationFactory);
            serviceCollection.AddHostedService(ImplementationFactory);
        }
    }
}
