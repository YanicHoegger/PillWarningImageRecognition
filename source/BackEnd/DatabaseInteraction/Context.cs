namespace DatabaseInteraction
{
    public class Context : IContext
    {
        public Context(string key, string endPoint, string dataBaseName, int? throughput)
        {
            Key = key;
            EndPoint = endPoint;
            DatabaseName = dataBaseName;
            Throughput = throughput;
        }

        public string Key { get; }
        public string EndPoint { get; }
        public string DatabaseName { get; }
        public int? Throughput { get; }
    }
}
