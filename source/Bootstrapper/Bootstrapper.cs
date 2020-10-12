using Bootstrapper.Interface;
using DatabaseInteraction;
using Domain;
using DrugCheckingCrawler;
using Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using ImageInteraction;

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

            if (configuration.ReadBool("MOCK"))
            {
                new Domain.Mock.DomainMockBootstrapper().ConfigureServices(services, configuration);
                return;
            }

#endif

            foreach (var composite in _compositeBootstrappers)
            {
                composite.ConfigureServices(services, configuration);
            }
        }
    }
}
