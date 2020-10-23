namespace DatabaseInteraction
{
    public interface IContext
    {
        string Key { get; }
        string EndPoint { get; }
        string DatabaseName { get; }
        string ContainerId { get; }
        int? Throughput { get; }
    }
}
