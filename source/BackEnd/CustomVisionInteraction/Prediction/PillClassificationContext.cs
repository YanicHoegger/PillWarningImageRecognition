using Microsoft.Extensions.Configuration;
using System;
using Utilities;

namespace CustomVisionInteraction.Prediction
{
    public class PillClassificationContext : IPillClassificationContext
    {
        private const string KeyConfigurationKey = "CLASSIFICATION_KEY";
        private const string EndPointConfigurationKey = "CLASSIFICATION_ENDPOINT";
        private const string ProjectIdConfigurationKey = "CLASSIFICATION_PROJECT_ID";
        private const string PublisherModelNameConfigurationKey = "CLASSIFICATION_PUBLISHER_MODEL";

        public PillClassificationContext(IConfiguration configuration)
        {
            Key = configuration.GetNotEmptyValue(KeyConfigurationKey);
            EndPoint = configuration.GetNotEmptyValue(EndPointConfigurationKey);
            ProjectId = Guid.Parse(configuration.GetNotEmptyValue(ProjectIdConfigurationKey));
            PublisherModelName = configuration.GetNotEmptyValue(PublisherModelNameConfigurationKey);
        }

        public string Key { get; }
        public string EndPoint { get; }
        public Guid ProjectId { get; }
        public string PublisherModelName { get; }
    }
}
