using Bootstrapper.Interface;
using Domain.Interface;
using Domain.Prediction;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Domain
{
    public class DomainBootstrapper : IBootstrapper
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IPredicition, Prediction.Prediction>();
            services.AddSingleton<IPillColorAnalyzer, PillColorAnalyzer>();
            services.AddSingleton<IProbabilityToLikelinessConverter, ProbabilityToLikelinessConverter>();
            services.AddSingleton<IPillRecognizer, PillRecognizer>();
            services.AddSingleton<IDrugCheckingSourceFactory, DrugCheckingSourceFactory>();
        }
    }
}
