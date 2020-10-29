using System;
using System.Collections.Generic;
using DatabaseInteraction;
using Domain;
using DrugCheckingCrawler;
using ImageInteraction;
using Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ManipulationClient
{
    public static class ServiceConfigurator
    {
        private static readonly List<(string configValue, Action<IServiceCollection> registration)> _executer = new List<(string, Action<IServiceCollection>)>
        {
            ("Update", services => services.AddSingleton<IExecuter, DrugCheckingUpdater>()),
            ("Crawl", services => services.AddSingleton<IExecuter, DrugCheckingCrawler>()),
            ("CleanPrediction", services => services.AddSingleton<IExecuter, PredictedImagesCleaner>())
        };

        public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            DomainBootstrapper.ConfigureServiceForManipulation(services);
            DatabaseBootstrapper.ConfigureServices(services, configuration);
            ImageInteractionBootstrapper.ConfigureServicesForManipulation(services, configuration);
            ResourceCrawlerBootstrapper.ConfigureServices(services);

            foreach ((string configValue, Action<IServiceCollection> registration) in _executer)
            {
                if (configuration.ReadBool(configValue))
                    registration(services);
            }
        }
    }
}
