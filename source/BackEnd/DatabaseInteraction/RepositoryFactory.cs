using DatabaseInteraction.Interface;
using Microsoft.Extensions.Logging;

namespace DatabaseInteraction
{
    public class RepositoryFactory : RepositoryFactoryBase
    {
        public RepositoryFactory(IContext context, ILogger<RepositoryFactory> logger) 
            : base(context, logger)
        {
        }

        protected override IRepository<T> OnCreate<T>()
        {
            return new Repository<T>(Container);
        }
    }
}
