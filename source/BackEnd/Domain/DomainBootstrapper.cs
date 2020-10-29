using Domain.DrugCheckingSource;
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

            services.AddSingleton<IFactory, Factory>();
            services.AddSingleton<IPillColorAnalyzer, PillColorAnalyzer>();

            services.AddSingleton<ResourceCrawler>();
            services.AddSingleton<CrawlerInformationHandler>();
            services.AddSingleton<StorageHandler>();
            services.AddSingleton<Updater>();

            services.AddSingleton<IPredictedImagesCleaner, PredictedImagesCleaner>();

            services.AddSingleton<IProbabilityToLikelinessConverter, ProbabilityToLikelinessConverter>();
        }
    }
}
