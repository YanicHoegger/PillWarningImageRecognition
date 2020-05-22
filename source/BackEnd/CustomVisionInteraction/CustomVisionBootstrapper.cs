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
            services.AddScoped<IComputerVisionCommunication, ComputerVisionCommunication>();
            services.AddSingleton<IPillDetectionContext, PillDetectionContext>();
            services.AddScoped<IPillDetectionCommunication, PillDetectionCommunication>();
            services.AddScoped<IPillDetection, PillDetection>();
            services.AddTransient<ICroppingService, CroppingService>();
            services.AddScoped<IColorAnalyzer, ColorAnalyzer.ColorAnalyzer>();

            services.AddSingleton<IPillClassificationContext, PillClassificationContext>();
            services.AddScoped<IPillClassificationCommunication, PillClassificationCommunication>();
            services.AddScoped<IPillClassification, PillClassification>();
            services.AddScoped<IPrediction, PredictionFacade>();
        }
    }
}
