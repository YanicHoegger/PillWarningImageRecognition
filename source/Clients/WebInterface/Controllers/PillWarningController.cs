using Clients.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WebInterface.Services;

namespace WebInterface.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PillWarningController : Controller
    {
        private readonly ILogger<PillWarningController> _logger;
        private readonly IPillWarningService _pillWarningService;

        public PillWarningController(ILogger<PillWarningController> logger, IPillWarningService pillWarningService)
        {
            _logger = logger;
            _pillWarningService = pillWarningService;
        }

        [HttpPost]
        public async Task<PredictionResult> Post(IFormFile file)
        {
            _logger.LogInformation("New pill warning request");

            try
            {
                return await _pillWarningService.GetPillWarnings(file.OpenReadStream());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not make prediction");
                return new PredictionResult();
            }
        }
    }
}
