using DatabaseInteraction.Interface;
using Domain.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WebInterface.Shared;

namespace WebInterface.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PillWarningController : ControllerBase
    {
        private readonly ILogger<PillWarningController> _logger;
        private readonly IPredicition _predicition;

        public PillWarningController(ILogger<PillWarningController> logger, IPredicition predicition)
        {
            _logger = logger;
            _predicition = predicition;
        }

        [HttpPost]
        public async Task<PredictionResult> Post(IFormFile file)
        {
            _logger.LogInformation("New pill warning request");

            IPredictionResult prediction;
            try
            {
                var image = await ReadImage(file);
                prediction = await _predicition.Predict(image);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not make prediction");
                return new PredictionResult();
            }

            return Convert(prediction);
        }

        private static async Task<byte[]> ReadImage(IFormFile file)
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
