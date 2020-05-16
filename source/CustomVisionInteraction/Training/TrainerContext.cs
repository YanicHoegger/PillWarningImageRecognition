using CustomVisionInteraction.Interface;
using Utilities;
using Microsoft.Extensions.Configuration;
using System;

namespace CustomVisionInteraction.Training
{
    public class TrainerContext : ITrainerContext
    {
        private const string _keyConfigurationKey = "TRAINER_KEY";
        private const string _endPointConfigurationKey = "TRAINER_END_POINT";
        private const string _projectIdConfigurationKey = "TRAINER_PROJECT_ID";

        public TrainerContext(IConfiguration configuration)
        {
            Key = configuration.GetNotEmptyValue(_keyConfigurationKey);
            EndPoint = configuration.GetNotEmptyValue(_endPointConfigurationKey);
            ProjectId = Guid.Parse(configuration.GetNotEmptyValue(_projectIdConfigurationKey));
        }

        public string Key { get; }

        public string EndPoint { get; }

        public Guid ProjectId { get; }
    }
}
