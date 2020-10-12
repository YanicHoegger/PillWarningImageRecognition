using Bootstrapper.Interface;
using ImageInteraction.Classification;
using ImageInteraction.ColorAnalyzer;
using ImageInteraction.Detection;
using ImageInteraction.ImageManipulation;
using ImageInteraction.Interface;
using ImageInteraction.Training;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Utilities;

namespace ImageInteraction
{
    public class CustomVisionBootstrapper : IBootstrapper
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ITrainerContext, TrainerContext>();
            services.AddHostedSingletonService<ITrainerCommunicator, TrainerCommunicator>();
            services.AddScoped<IClassificationTrainer, ClassificationTrainer>();

            services.AddSingleton<IVisionContext, VisionContext>();
            services.AddSingleton<IComputerVisionCommunication, ComputerVisionCommunication>();
            services.AddSingleton<DetectionContext>();
            services.AddSingleton<IDetectionCommunication, DetectionCommunication>(sp => new DetectionCommunication(sp.GetRequiredService<DetectionContext>()));
            services.AddSingleton<IDetector, Detector>();
            services.AddTransient<ICroppingService, CroppingService>();
            services.AddSingleton<IColorAnalyzer, ColorAnalyzer.ColorAnalyzer>();

            services.AddSingleton<PillClassificationContext>();
            services.AddSingleton<IPillClassificationCommunication, PillClassificationCommunication>(sp => new PillClassificationCommunication(sp.GetRequiredService<PillClassificationContext>()));
            services.AddSingleton<IClassifier, PillClassification>();
        }
    }
}
