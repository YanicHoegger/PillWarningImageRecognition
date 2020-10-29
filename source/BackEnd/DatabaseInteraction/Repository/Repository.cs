using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseInteraction.Entity;
using DatabaseInteraction.Interface;

namespace DatabaseInteraction.Repository
{
    public class Repository<TInterface, TImplementation> : RepositoryBase<TInterface, TImplementation> 
        where TInterface : IEntity
        where TImplementation : Entity.Entity, TInterface
    {
        public Repository(ContainerFactory<TInterface> containerFactory, EntityFactory entityFactory) 
            : base(containerFactory, entityFactory)
        {
        }

        protected override IAsyncEnumerable<TImplementation> GetInternal()
        {
            return RetrieveList();
        }

        protected override IAsyncEnumerable<TImplementation> GetInternal(Func<IQueryable<TImplementation>, IQueryable<TImplementation>> queries)
        {
            return RetrieveList(queries);
        }
    }
}
