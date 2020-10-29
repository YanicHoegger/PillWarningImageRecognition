using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Domain.DrugCheckingSource;

namespace ManipulationClient
{
    public class DrugCheckingUpdater : IExecuter
    {
        public async Task Execute(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            var updater = serviceProvider.GetService<Updater>();
            await updater.UpdateResources();
        }
    }
}
