using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Threading.Tasks;
using DatabaseInteraction.Interface;

namespace DatabaseInteraction
{
    /// <summary>
    /// Use this class when for a long running usage of the implementation. 
    /// The first call will be very slow but all others will be fast. 
    /// If you use a short living application use <see cref="Repository"/>
    /// </summary>
    public class CachedRepository<T> : RepositoryBase<T> 
        where T : Entity, new()
    {
        private List<T> _cach;

        public CachedRepository(Container container) 
            : base(container)
        {
        }

        public override async Task<List<T>> Get()
        {
            if (_cach == null)
                _cach = await GetFromDb();

            return _cach;
        }

        protected override void OnInsert()
        {
            UpdateCach();
        }

        private void UpdateCach()
        {
            Task.Run(async () =>
            {
                _cach = await GetFromDb();
            });
        }
    }
}
