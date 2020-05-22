using CustomVisionInteraction.Interface;
using System;

namespace CustomVisionInteraction.Prediction
{
    public class PredictionContext : Context, IPredictionContext
    {
        public PredictionContext(string key, string endPoint, Guid projectId, string publisherModelName)
            : base(key, endPoint, projectId)
        {
            PublisherModelName = publisherModelName;
        }

        public string PublisherModelName { get; }
    }
}
