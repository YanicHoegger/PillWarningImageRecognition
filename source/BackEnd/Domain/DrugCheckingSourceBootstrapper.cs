using Bootstrapper.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Domain
{
    public class DrugCheckingSourceBootstrapper : IBootstrapper
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<DrugCheckingSourceManager>();
            services.AddSingleton<CrawlerInformationHandler>();
            services.AddSingleton<DrugCheckingSourceHandler>();
        }
    }
}
