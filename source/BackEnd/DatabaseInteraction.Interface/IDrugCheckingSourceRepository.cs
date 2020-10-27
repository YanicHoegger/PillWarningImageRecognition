using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace DatabaseInteraction.Interface
{
    public interface IDrugCheckingSourceRepository : IRepository<DrugCheckingSource>
    {
        Task<bool> Contains(DrugCheckingSource drugCheckingSource);

        Task<DrugCheckingSource> SingleOrDefault(DrugCheckingSource drugCheckingSource);

        IAsyncEnumerable<DrugCheckingSource> GetSameColor(Color color, int take);

        IAsyncEnumerable<DrugCheckingSource> GetSameTagName(string tagName);
    }
}
