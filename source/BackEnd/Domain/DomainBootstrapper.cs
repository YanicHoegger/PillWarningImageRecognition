using Domain.Interface;
using Domain.Prediction;
using Microsoft.Extensions.DependencyInjection;

namespace Domain
{
    public static class DomainBootstrapper
    {
        public static void ConfigureServicesForPrediction(IServiceCollection services)
        {
            services.AddSingleton<IPredicition, Prediction.Prediction>();
            services.AddSingleton<IPillColorAnalyzer, PillColorAnalyzer>();
            services.AddSingleton<IProbabilityToLikelinessConverter, ProbabilityToLikelinessConverter>();
            services.AddSingleton<IClassificationPillRecognizer, ClassificationPillRecognizer>();
        }

        public static void ConfigureServiceForManipulation(IServiceCollection services)
        {
            services.AddSingleton<IImagePillRecognizer, ImagePillRecognizer>();

            services.AddSingleton<IDrugCheckingSourceFactory, DrugCheckingSourceFactory>();

            services.AddSingleton<DrugCheckingSourceManager>();
            services.AddSingleton<CrawlerInformationHandler>();
            services.AddSingleton<DrugCheckingSourceHandler>();

            services.AddSingleton<IPredictedImagesCleaner, PredictedImagesCleaner>();
        }
    }
}
