using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using DatabaseInteraction.Interface;

namespace DatabaseInteraction.Repository
{
    public class DrugCheckingSourceRepository : Repository<DrugCheckingSource>, IDrugCheckingSourceRepository
    {
        private readonly DrugCheckingSourceQueries _queries = new DrugCheckingSourceQueries();

        public DrugCheckingSourceRepository(ContainerFactory<DrugCheckingSource> containerFactory) 
            : base(containerFactory)
        {
        }

        public async Task<bool> Contains(DrugCheckingSource drugCheckingSource)
        {
            return await Get(_queries.GetSameItemQuery(drugCheckingSource))
                .AnyAsync();
        }

        public async Task<DrugCheckingSource> SingleOrDefault(DrugCheckingSource drugCheckingSource)
        {
            return await Get(_queries.GetSameItemQuery(drugCheckingSource))
                .SingleOrDefaultAsync();
        }

        public IAsyncEnumerable<DrugCheckingSource> GetSameColor(Color color, int take)
        {
            return Get(_queries.GetColorQuery(color, take));
        }

        public IAsyncEnumerable<DrugCheckingSource> GetSameTagName(string tagName)
        {
            return Get(_queries.GetTagNameQuery(tagName));
        }
    }
}
