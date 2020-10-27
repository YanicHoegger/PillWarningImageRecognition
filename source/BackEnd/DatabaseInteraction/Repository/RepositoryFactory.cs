using DatabaseInteraction.Interface;
using Microsoft.Azure.Cosmos;

namespace DatabaseInteraction.Repository
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly Container _container;

        public RepositoryFactory(Container container)
        {
            _container = container;
        }

        public IRepository<T> Create<T>() where T : Entity, new()
        {
            return new Repository<T>(_container);
        }

        public IDrugCheckingSourceRepository CreateDrugCheckingSourceRepository()
        {
            return new DrugCheckingSourceRepository(_container);
        }
    }
}
