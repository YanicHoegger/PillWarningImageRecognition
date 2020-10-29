using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using DatabaseInteraction.Entity;
using DatabaseInteraction.Interface;

namespace DatabaseInteraction.Repository
{
    public class DrugCheckingSourceRepository : Repository<IDrugCheckingSource, DrugCheckingSource>, IDrugCheckingSourceRepository
    {
        private readonly DrugCheckingSourceQueries _queries = new DrugCheckingSourceQueries();

        public DrugCheckingSourceRepository(ContainerFactory<IDrugCheckingSource> containerFactory, EntityFactory entityFactory) 
            : base(containerFactory, entityFactory)
        {
        }

        public async Task<bool> Contains(IDrugCheckingSource drugCheckingSource)
        {
            return await GetInternal(_queries.GetSameItemQuery(drugCheckingSource))
                .AnyAsync();
        }

        public async Task<IDrugCheckingSource> SingleOrDefault(IDrugCheckingSource drugCheckingSource)
        {
            return await GetInternal(_queries.GetSameItemQuery(drugCheckingSource))
                .SingleOrDefaultAsync();
        }

        public IAsyncEnumerable<IDrugCheckingSource> GetSameColor(Color color, int take)
        {
            return GetInternal(_queries.GetColorQuery(color, take));
        }

        public IAsyncEnumerable<IDrugCheckingSource> GetSameTagName(string tagName)
        {
            return GetInternal(_queries.GetTagNameQuery(tagName));
        }
    }
}
