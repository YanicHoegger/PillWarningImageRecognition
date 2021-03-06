﻿using System;
using Microsoft.Extensions.Configuration;
using Utilities;

namespace ImageInteraction.Detection
{
    public class DetectionContext : IClassificationContext
    {
        private const string _keyConfigurationKey = "DETECTION_KEY";
        private const string _endPointConfigurationKey = "DETECTION_ENDPOINT";
        private const string _projectIdConfigurationKey = "DETECTION_PROJECT_ID";
        private const string _publisherModelNameConfigurationKey = "DETECTION_PUBLISHER_MODEL";

        public DetectionContext(IConfiguration configuration)
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
