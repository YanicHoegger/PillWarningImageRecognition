namespace DatabaseInteraction
{
    public interface IContext
    {
        string ConnectionString { get; }
        string DatabaseName { get; }
    }
}
