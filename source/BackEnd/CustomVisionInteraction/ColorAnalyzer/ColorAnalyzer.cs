using CustomVisionInteraction.Interface;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace CustomVisionInteraction.ColorAnalyzer
{
    public class ColorAnalyzer : IColorAnalyzer
    {
        private readonly IComputerVisionCommunication _computerVisionCommunication;
        private readonly IPillDetection _pillDetection;
        private readonly ICroppingService _croppingService;

        private readonly Dictionary<string, Color> _colorMapping = new Dictionary<string, Color>
        {
            //According to documentation this are the only colors that can appear
            //https://docs.microsoft.com/en-us/azure/cognitive-services/computer-vision/concept-detecting-color-schemes
            { "Black", Color.Black },
            { "Blue", Color.Blue },
            { "Brown", Color.Brown },
            { "Grey", Color.Gray },
            { "Green", Color.Green },
            { "Orange", Color.Orange },
            { "Pink", Color.Pink },
            { "Purple", Color.Purple },
            { "Red", Color.Red },
            { "Teal", Color.Teal },
            { "White", Color.White },
            { "Yellow", Color.Yellow }
        };

        public ColorAnalyzer(IComputerVisionCommunication computerVisionCommunication, IPillDetection pillDetection, ICroppingService croppingService)
        {
            _computerVisionCommunication = computerVisionCommunication;
            _pillDetection = pillDetection;
            _croppingService = croppingService;
        }

        public async Task<Color> GetColor(byte[] image)
        {
            var croppedImage = await CropImage(image);

            var result = await _computerVisionCommunication.GetComputerVision(new MemoryStream(croppedImage), new[] { VisualFeatureTypes.Color });

            return _colorMapping[result.Color.DominantColorForeground];
        }

        private async Task<byte[]> CropImage(byte[] image)
        {
            var (hasDetection, boundingBox) = await _pillDetection.GetBestDetection(image);

            if (hasDetection)
            {
                return _croppingService.CropImage(image, boundingBox);
            }
            else
            {
                return image;
            }
        }
    }
}
