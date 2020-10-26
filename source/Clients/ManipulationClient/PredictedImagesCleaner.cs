using System;
using System.Threading.Tasks;
using Domain.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ManipulationClient
{
    public class PredictedImagesCleaner : IExecuter
    {
        public async Task Execute(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            var cleaner = serviceProvider.GetService<IPredictedImagesCleaner>();
            await cleaner.CleanPredictions();
        }
    }
}
