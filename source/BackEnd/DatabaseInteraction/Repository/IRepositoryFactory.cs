using DatabaseInteraction.Interface;

namespace DatabaseInteraction.Repository
{
    public interface IRepositoryFactory
    {
        IRepository<T> Create<T>() where T : Entity.Entity, new();

        IDrugCheckingSourceRepository CreateDrugCheckingSourceRepository();
    }
}
