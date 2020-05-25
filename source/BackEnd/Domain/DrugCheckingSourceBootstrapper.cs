using Bootstrapper.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Domain
{
    public class DrugCheckingSourceBootstrapper : IBootstrapper
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<DrugCheckingSourceManager>();
            services.AddScoped<CrawlerInformationHandler>();
            services.AddScoped<DrugCheckingSourceHandler>();
        }
    }
}
