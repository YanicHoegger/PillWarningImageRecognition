using Bootstrapper.Interface;
using CustomVisionInteraction.ColorAnalyzer;
using CustomVisionInteraction.Interface;
using CustomVisionInteraction.Prediction;
using CustomVisionInteraction.Training;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Utilities;

namespace CustomVisionInteraction
{
    public class CustomVisionBootstrapper : IBootstrapper
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ITrainerContext, TrainerContext>();
            services.AddHostedSingletonService<ITrainerCommunicator, TrainerCommunicator>();
            services.AddScoped<IPillRecognitionTrainer, PillRecognitionTrainer>();

            services.AddSingleton<IVisionContext, VisionContext>();
            services.AddSingleton<IComputerVisionCommunication, ComputerVisionCommunication>();
            services.AddSingleton<PillDetectionContext>();
            services.AddSingleton<IPillDetectionCommunication, PillDetectionCommunication>(sp => new PillDetectionCommunication(sp.GetRequiredService<PillDetectionContext>()));
            services.AddSingleton<IDetector, PillDetection>();
            services.AddTransient<ICroppingService, CroppingService>();
            services.AddSingleton<IColorAnalyzer, ColorAnalyzer.ColorAnalyzer>();

            services.AddSingleton<PillClassificationContext>();
            services.AddSingleton<IPillClassificationCommunication, PillClassificationCommunication>(sp => new PillClassificationCommunication(sp.GetRequiredService<PillClassificationContext>()));
            services.AddSingleton<IClassifier, PillClassification>();
        }
    }
}
