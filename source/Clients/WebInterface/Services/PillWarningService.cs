using Clients.Shared;
using Domain.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebInterface.Services
{
    public class PillWarningService : IPillWarningService
    {
        private readonly ILogger<PillWarningService> _logger;
        private readonly IPredicition _prediction;

        public PillWarningService(ILogger<PillWarningService> logger, IPredicition prediction)
        {
            _logger = logger;
            _prediction = prediction;
        }

        public async Task<PredictionResult> GetPillWarnings(Stream image)
        {
            _logger.LogInformation("New pill warning request");

            IPredictionResult prediction;
            try
            {
                var imageByteArray = await ReadImage(image);
                prediction = await _prediction.Predict(imageByteArray);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not make prediction");
                return new PredictionResult();
            }

            return Convert(prediction);
        }

        private static async Task<byte[]> ReadImage(Stream file)
        {
            var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            return memoryStream.ToArray();
        }

        private static PredictionResult Convert(IPredictionResult toConvert)
        {
            return Converter.ToPredictionResult(toConvert);
        }

        /// <summary>
        /// Use this method for generating the text that would be used in a mock
        /// </summary>
#pragma warning disable IDE0051 // Remove unused private members
        private static string GetTextFromPredictionResult(IPredictionResult predictionResult)
#pragma warning restore IDE0051 // Remove unused private members
        {
            //return JsonSerializer.Serialize(predictionResult);
            return JsonSerializer.Serialize(Convert(predictionResult));
        }
    }
}
