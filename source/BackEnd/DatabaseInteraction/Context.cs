namespace DatabaseInteraction
{
    public class Context : IContext
    {
        public Context(string key, string endPoint, string dataBaseName, string containerId, int? throughput)
        {
            Key = key;
            EndPoint = endPoint;
            DatabaseName = dataBaseName;
            ContainerId = containerId;
            Throughput = throughput;
        }

        public string Key { get; }
        public string EndPoint { get; }
        public string DatabaseName { get; }
        public string ContainerId { get; }
        public int? Throughput { get; }
    }
}
