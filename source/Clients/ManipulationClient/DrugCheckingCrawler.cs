using Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Domain.DrugCheckingSource;

namespace ManipulationClient
{
    public class DrugCheckingCrawler : IExecuter
    {
        public async Task Execute(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            var crawler = serviceProvider.GetService<ResourceCrawler>();
            await crawler.SetUpResources();
        }
    }
}
