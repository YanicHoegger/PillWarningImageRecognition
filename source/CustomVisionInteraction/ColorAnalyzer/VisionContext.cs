using CustomVisionInteraction.Interface;
using Utilities;
using Microsoft.Extensions.Configuration;

namespace CustomVisionInteraction.ColorAnalyzer
{
    public class VisionContext : IVisionContext
    {
        private const string _keyConfigurationKey = "VISION_KEY";
        private const string _endPointConfigurationKey = "VISION_END_POINT";

        public VisionContext(IConfiguration configuration)
        {
            Key = configuration.GetNotEmptyValue(_keyConfigurationKey);
            EndPoint = configuration.GetNotEmptyValue(_endPointConfigurationKey);
        }

        public string Key { get; }
        public string EndPoint { get; }
    }
}
