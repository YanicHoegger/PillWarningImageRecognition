﻿using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace CustomVisionInteraction.Prediction
{
    public class ColorAnalyzer
    {
        private readonly IComputerVisionCommunication _computerVisionCommunication;
        private readonly PillDetection _pillDetection;
        private readonly Dictionary<string, Color> _colorMapping = new Dictionary<string, Color>
        {
            //According to documentation this are the only colors that can appear
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

        public ColorAnalyzer(IComputerVisionCommunication computerVisionCommunication, PillDetection pillDetection)
        {
            _computerVisionCommunication = computerVisionCommunication;
            _pillDetection = pillDetection;
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
                return new CroppingService().CropImage(image, boundingBox);
            }
            else
            {
                return image;
            }
        }
    }
}