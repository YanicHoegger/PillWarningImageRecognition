﻿using Clients.Shared;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace MobileInterface.Services.Mock
{
    public class PredictionServiceMock : IPredictionService
    {
        public Task<PredictionResult> Predict(Stream image, string name)
        {
            var predictionResult = JsonSerializer.Deserialize<PredictionResult>(MockResources.MockData);
            return Task.FromResult(predictionResult);
        }
    }
}
