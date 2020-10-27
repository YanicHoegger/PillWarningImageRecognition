using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Utilities
{
    public static class ServiceCollectionExtensions
    {
        //TODO: Add decorators for logging and throw if not started
        public static void AddHostedSingletonService<TInterface, TImplementation>(this IServiceCollection serviceCollection)
            where TInterface : class
            where TImplementation : class, TInterface, IHostedService
        {
            serviceCollection.AddSingleton<TImplementation>();

            serviceCollection.RegisterAsInterfaceAndHostedService<TInterface, TImplementation>();
        }

        public static void AddHostedSingletonService<TImplementation>(this IServiceCollection serviceCollection)
            where TImplementation : class, IHostedService
        {
            serviceCollection.AddSingleton<TImplementation>();

            serviceCollection.AddHostedService(GetImplementationFactory<TImplementation>());
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
            serviceCollection.AddSingleton<TInterface, TImplementation>(GetImplementationFactory<TImplementation>());
            serviceCollection.AddHostedService(GetImplementationFactory<TImplementation>());
        }

        private static Func<IServiceProvider, T> GetImplementationFactory<T>()
        {
            return x => x.GetRequiredService<T>();
        }
    }
}
