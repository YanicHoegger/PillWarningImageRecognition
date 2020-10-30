using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.Extensions.Logging;
using Utilities.Aspects;

namespace Utilities
{
    public static class ServiceCollectionExtensions
    {
        //TODO: Add decorators for logging
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
            where TInterface : class 
            where TImplementation : class, TInterface, IHostedService
        {
            TInterface proxy = null;
            HostedServiceState<TImplementation> hostedService = null;

            void CheckCreated(IServiceProvider serviceProvider)
            {
                if (proxy == null || hostedService == null)
                {
                    (proxy, hostedService) =
                        HostedServiceDecorator<TInterface>.Create(
                            serviceProvider.GetRequiredService<TImplementation>(),
                            serviceProvider.GetRequiredService<ILogger<TImplementation>>());
                }
            }

            serviceCollection.AddSingleton(serviceProvider =>
            {
                CheckCreated(serviceProvider);
                return proxy;
            });
            serviceCollection.AddHostedService(serviceProvider =>
            {
                CheckCreated(serviceProvider);
                return hostedService;
            });
        }
    }
}
