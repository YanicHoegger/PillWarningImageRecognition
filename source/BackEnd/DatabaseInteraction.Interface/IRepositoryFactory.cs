namespace DatabaseInteraction.Interface
{
    public interface IRepositoryFactory
    {
        IRepository<T> Create<T>() where T : Entity, new();
    }
}
