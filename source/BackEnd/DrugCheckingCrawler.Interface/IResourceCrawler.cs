using System.Threading.Tasks;

namespace DrugCheckingCrawler.Interface
{
    public interface IResourceCrawler
    {
        //TODO: Use IAsyncEnumerable 
        Task<ICrawlerResult> Crawl(int startIndex);
    }
}
