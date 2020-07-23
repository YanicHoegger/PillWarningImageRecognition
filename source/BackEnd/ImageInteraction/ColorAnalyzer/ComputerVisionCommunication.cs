using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace ImageInteraction.ColorAnalyzer
{
    public class ComputerVisionCommunication : IComputerVisionCommunication
    {
        private readonly IVisionContext _context;

        public ComputerVisionCommunication(IVisionContext context)
        {
            _context = context;
        }

        public async Task<ImageAnalysis> GetComputerVision(Stream image, IList<VisualFeatureTypes> visualFeatureTypes)
        {
            var client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(_context.Key))
            {
                Endpoint = _context.EndPoint
            };

            return await client.AnalyzeImageInStreamAsync(image, visualFeatureTypes);
        }
    }
}
