using DatabaseInteraction.Interface;

namespace DatabaseInteraction
{
    public interface IRepositoryFactory
    {
        IRepository<T> Create<T>() where T : Entity, new();
    }
}
