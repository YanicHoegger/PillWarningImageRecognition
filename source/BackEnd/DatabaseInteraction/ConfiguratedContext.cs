using Utilities;
using Microsoft.Extensions.Configuration;

namespace DatabaseInteraction
{
    public class ConfiguratedContext : IContext
    {
        private const string KeyConfigKey = "DATABASE_KEY";
        private const string EndPointConfigKey = "DATABASE_END_POINT";
        private const string DatabaseNameConfigKey = "DATABASE_NAME";
        private const string ContainerIdConfigKey = "DATABASE_CONTAINER_ID";

        public ConfiguratedContext(IConfiguration configuration)
        {
            Key = configuration.GetNotEmptyValue(KeyConfigKey);
            EndPoint = configuration.GetNotEmptyValue(EndPointConfigKey);
            DatabaseName = configuration.GetNotEmptyValue(DatabaseNameConfigKey);
            ContainerId = configuration.GetNotEmptyValue(ContainerIdConfigKey);
        }

        public string Key { get; }
        public string EndPoint { get; }
        public string DatabaseName { get; }
        public string ContainerId { get; }
    }
}
