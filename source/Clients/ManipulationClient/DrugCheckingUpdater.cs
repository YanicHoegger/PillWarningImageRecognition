using Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace ManipulationClient
{
    public class DrugCheckingUpdater : IExecuter
    {
        public async Task Execute(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            var manager = serviceProvider.GetService<DrugCheckingSourceManager>();
            await manager.UpdateResources();
        }
    }
}
