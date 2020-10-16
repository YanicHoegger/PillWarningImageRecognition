using System.Threading.Tasks;
using DatabaseInteraction.Interface;
using DrugCheckingCrawler.Interface;

namespace Domain
{
    public interface IDrugCheckingSourceFactory
    {
        Task<DrugCheckingSource> Create(ICrawlerResultItem item);
    }
}
