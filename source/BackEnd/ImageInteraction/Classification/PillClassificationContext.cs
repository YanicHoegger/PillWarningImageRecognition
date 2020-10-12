using System;
using Microsoft.Extensions.Configuration;
using Utilities;

namespace ImageInteraction.Classification
{
    public class PillClassificationContext : IClassificationContext
    {
        private const string _keyConfigurationKey = "CLASSIFICATION_KEY";
        private const string _endPointConfigurationKey = "CLASSIFICATION_ENDPOINT";
        private const string _projectIdConfigurationKey = "CLASSIFICATION_PROJECT_ID";
        private const string _publisherModelNameConfigurationKey = "CLASSIFICATION_PUBLISHER_MODEL";

        public PillClassificationContext(IConfiguration configuration)
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
