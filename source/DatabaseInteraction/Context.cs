namespace DatabaseInteraction
{
    public class Context : IContext
    {
        public Context(string connectionString, string dataBaseName)
        {
            ConnectionString = connectionString;
            DatabaseName = dataBaseName;
        }

        public string ConnectionString { get; }
        public string DatabaseName { get; }
    }
}
