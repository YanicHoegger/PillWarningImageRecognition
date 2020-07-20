using CustomVisionInteraction.Interface;
using Microsoft.Extensions.Configuration;
using System;
using Utilities;

namespace CustomVisionInteraction.ColorAnalyzer
{
    public class PillDetectionContext : IPredictionContext
    {
        private const string _keyConfigurationKey = "DETECTION_KEY";
        private const string _endPointConfigurationKey = "DETECTION_ENDPOINT";
        private const string _projectIdConfigurationKey = "DETECTION_PROJECT_ID";
        private const string _publisherModelNameConfigurationKey = "DETECTION_PUBLISHER_MODEL";

        public PillDetectionContext(IConfiguration configuration)
        {
            Key = configuration.GetNotEmptyValue(_keyConfigurationKey);
            EndPoint = configuration.GetNotEmptyValue(_endPointConfigurationKey);
            ProjectId = Guid.Parse(configuration.GetNotEmptyValue(_projectIdConfigurationKey));
            PublisherModelName = configuration.GetNotEmptyValue(_publisherModelNameConfigurationKey);
        }

        public string Key { get; }
        public string EndPoint { get; }
        public Guid ProjectId { get; }
        public string PublisherModelName { get; }
    }
}
