using Clients.Shared;
using DatabaseInteraction.Interface;
using Domain.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebInterface.Services
{
    public class PillWarningService : IPillWarningService
    {
        private readonly ILogger<PillWarningService> _logger;
        private readonly IPredicition _predicition;

        public PillWarningService(ILogger<PillWarningService> logger, IPredicition predicition)
        {
            _logger = logger;
            _predicition = predicition;
        }

        public async Task<PredictionResult> GetPillWarnings(Stream image)
        {
            _logger.LogInformation("New pill warning request");

            IPredictionResult prediction;
            try
            {
                var imageByteArray = await ReadImage(image);
                prediction = await _predicition.Predict(imageByteArray);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not make prediction");
                return new PredictionResult();
            }

            if (prediction == null)
            {
                _logger.LogInformation("No result out of prediction");
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
            return new PredictionResult
            {
                SameInprint = toConvert.TagFindings.Select(Convert),
                SameColor = toConvert.ColorFindings.Select(Convert)
            };
        }

        private static PillWarning Convert(DrugCheckingSource toConvert)
        {
            //TODO: Use AutoMapper or similar
            return new PillWarning
            {
                Name = toConvert.Name,
                Color = toConvert.Color,
                Creation = toConvert.Creation,
                Image = toConvert.Image,
                PdfLocation = toConvert.PdfLocation
            };
        }
    }
}
