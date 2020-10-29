using System.Threading.Tasks;
using DatabaseInteraction.Interface;
using DrugCheckingCrawler.Interface;

namespace Domain.DrugCheckingSource
{
    public interface IFactory
    {
        Task<IDrugCheckingSource> Create(ICrawlerResultItem item);
    }
}
