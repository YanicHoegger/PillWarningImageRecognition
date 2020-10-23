using Microsoft.Extensions.Configuration;
using Utilities;

namespace DatabaseInteraction
{
    public class ConfiguratedContext : IContext
    {
        private const string _keyConfigKey = "DATABASE_KEY";
        private const string _endPointConfigKey = "DATABASE_END_POINT";
        private const string _databaseNameConfigKey = "DATABASE_NAME";
        private const string _containerIdConfigKey = "DATABASE_CONTAINER_ID";
        private const string _throughputKey = "DATABASE_THROUGHPUT";

        public ConfiguratedContext(IConfiguration configuration)
        {
            Key = configuration.GetNotEmptyValue(_keyConfigKey);
            EndPoint = configuration.GetNotEmptyValue(_endPointConfigKey);
            DatabaseName = configuration.GetNotEmptyValue(_databaseNameConfigKey);
            ContainerId = configuration.GetNotEmptyValue(_containerIdConfigKey);

            Throughput = int.TryParse(configuration[_throughputKey], out var throughput) ? throughput : (int?) null;
        }

        public string Key { get; }
        public string EndPoint { get; }
        public string DatabaseName { get; }
        public string ContainerId { get; }
        public int? Throughput { get; }
    }
}
