using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace DatabaseInteraction.Interface
{
    public interface IDrugCheckingSourceRepository : IRepository<IDrugCheckingSource>
    {
        Task<bool> Contains(IDrugCheckingSource drugCheckingSource);

        Task<IDrugCheckingSource> SingleOrDefault(IDrugCheckingSource drugCheckingSource);

        IAsyncEnumerable<IDrugCheckingSource> GetSameColor(Color color, int take);

        IAsyncEnumerable<IDrugCheckingSource> GetSameTagName(string tagName);
    }
}
