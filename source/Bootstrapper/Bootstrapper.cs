using Bootstrapper.Interface;
using CustomVisionInteraction;
using DatabaseInteraction;
using Domain;
using DrugCheckingCrawler;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Bootstrapper
{
    public class Bootstrapper : IBootstrapper
    {
        private readonly IList<IBootstrapper> _compositeBootstrappers = new List<IBootstrapper>()
        {
            new DomainBootstrapper(),
            new DatabaseBootstrapper(),
            new ResourceCrawlerBootstrapper(),
            new CustomVisionBootstrapper()
        };

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
#if DEBUG

            var couldParse = bool.TryParse(configuration["MOCK"], out var isMock);
            if (couldParse && isMock)
            {
                new Domain.Mock.DomainMockBootstrapper().ConfigureServices(services, configuration);
                return;
            }

#endif

            foreach(var composite in _compositeBootstrappers)
            {
                composite.ConfigureServices(services, configuration);
            }
        }
    }
}
