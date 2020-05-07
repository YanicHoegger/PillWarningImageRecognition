﻿using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CustomVisionInteraction.Prediction
{
    public interface IComputerVisionCommunication
    {
        Task<ImageAnalysis> GetComputerVision(Stream image, IList<VisualFeatureTypes> visualFeatureTypes);
    }
}
