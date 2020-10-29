using ImageInteraction.Classification;
using ImageInteraction.ColorAnalyzer;
using ImageInteraction.Detection;
using ImageInteraction.ImageManipulation;
using ImageInteraction.Interface;
using ImageInteraction.PredictedImagesManager;
using ImageInteraction.Training;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Utilities;

namespace ImageInteraction
{
    public static class ImageInteractionBootstrapper
    {
        private const string _cleanPrediction = "CleanPrediction";
        private const string _crawl = "Crawl";

        public static void ConfigureServicesForPillRecognition(IServiceCollection services)
        {
            ConfigureForClassification(services);

            services.AddSingleton<DetectionContext>();
            services.AddSingleton<IDetectionCommunication, DetectionCommunication>(serviceProvider => new DetectionCommunication(serviceProvider.GetRequiredService<DetectionContext>()));
            services.AddSingleton<IDetector, Detector>();
            services.AddTransient<ICroppingService, CroppingService>();
        }

        public static void ConfigureServicesForManipulation(IServiceCollection services, IConfiguration configuration)
        {
            ConfigureForClassification(services);

            services.AddSingleton<TrainerContext>();

            var isCleaning = configuration.ReadBool(_cleanPrediction);
            if (isCleaning)
            {
                services.AddHostedSingletonService<PredictedImagesProviderBase, CachedPredictedImagesProvider>(serviceProvider => new CachedPredictedImagesProvider(serviceProvider.GetService<TrainerContext>()));
            }
            else
            {
                services.AddSingleton<PredictedImagesProviderBase, PredictedImagesProvider>(serviceProvider => new PredictedImagesProvider(serviceProvider.GetService<TrainerContext>()));
            }

            services.AddSingleton<IPredictedImagesManager, PredictedImagesManager.PredictedImagesManager>(serviceProvider =>
            {
                // ReSharper disable once ConvertToLambdaExpression
                return new PredictedImagesManager.PredictedImagesManager(serviceProvider.GetService<TrainerContext>(),
                    serviceProvider.GetService<PredictedImagesProviderBase>());
            });

            // ReSharper disable once InvertIf
            if (configuration.ReadBool(_crawl))
            {
                services.AddHostedSingletonService<ITrainerCommunicator, TrainerCommunicator>(serviceProvider => new TrainerCommunicator(serviceProvider.GetService<TrainerContext>()));
                services.AddSingleton<IClassificationTrainer, ClassificationTrainer>();
            }
        }

        private static void ConfigureForClassification(IServiceCollection services)
        {
            services.AddSingleton<PillClassificationContext>();

            services.AddSingleton<IClassifier, PillClassification>(serviceProvider => new PillClassification(serviceProvider.GetService<PillClassificationContext>()));

            services.AddSingleton<IVisionContext, VisionContext>();
            services.AddSingleton<IComputerVisionCommunication, ComputerVisionCommunication>();
            services.AddSingleton<IColorAnalyzer, ColorAnalyzer.ColorAnalyzer>();
        }
    }
}
