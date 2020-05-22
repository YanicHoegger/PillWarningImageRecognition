namespace DatabaseInteraction
{
    public class Context : IContext
    {
        public Context(string key, string endPoint, string dataBaseName, string containerId)
        {
            Key = key;
            EndPoint = endPoint;
            DatabaseName = dataBaseName;
            ContainerId = containerId;
        }

        public string Key { get; }
        public string EndPoint { get; }
        public string DatabaseName { get; }
        public string ContainerId { get; }
    }
}
