using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace ImageInteraction.ColorAnalyzer
{
    public interface IComputerVisionCommunication
    {
        Task<ImageAnalysis> GetComputerVision(Stream image, IList<VisualFeatureTypes> visualFeatureTypes);
    }
}
