using ImageInteraction.Classification;
using ImageInteraction.ColorAnalyzer;
using ImageInteraction.Detection;
using ImageInteraction.ImageManipulation;
using ImageInteraction.Interface;
using ImageInteraction.Training;
using Microsoft.Extensions.DependencyInjection;
using Utilities;

namespace ImageInteraction
{
    public static class CustomVisionBootstrapper
    {
        public static void ConfigureServicesForPillRecognition(IServiceCollection services)
        {
            ConfigureForClassification(services);

            services.AddSingleton<IVisionContext, VisionContext>();
            services.AddSingleton<IComputerVisionCommunication, ComputerVisionCommunication>();
            services.AddSingleton<DetectionContext>();
            services.AddSingleton<IDetectionCommunication, DetectionCommunication>(serviceProvider => new DetectionCommunication(serviceProvider.GetRequiredService<DetectionContext>()));
            services.AddSingleton<IDetector, Detector>();
            services.AddTransient<ICroppingService, CroppingService>();
            services.AddSingleton<IColorAnalyzer, ColorAnalyzer.ColorAnalyzer>();
        }

        public static void ConfigureServicesForManipulation(IServiceCollection services)
        {
            ConfigureForClassification(services);

            services.AddHostedSingletonService<IPredictedImagesManager, PredictedImagesManager.PredictedImagesManager>(serviceProvider => new PredictedImagesManager.PredictedImagesManager(serviceProvider.GetService<TrainerContext>()));

            services.AddSingleton<TrainerContext>();
            services.AddHostedSingletonService<ITrainerCommunicator, TrainerCommunicator>(serviceProvider => new TrainerCommunicator(serviceProvider.GetService<TrainerContext>()));
            services.AddSingleton<IClassificationTrainer, ClassificationTrainer>();
        }

        private static void ConfigureForClassification(IServiceCollection services)
        {
            services.AddSingleton<PillClassificationContext>();

            services.AddSingleton<IPillClassificationCommunication, PillClassificationCommunication>(sp =>
                new PillClassificationCommunication(sp.GetRequiredService<PillClassificationContext>()));

            services.AddSingleton<IClassifier, PillClassification>();
        }
    }
}
