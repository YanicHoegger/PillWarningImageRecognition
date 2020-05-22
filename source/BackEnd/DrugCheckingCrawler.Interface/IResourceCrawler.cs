using System.Threading.Tasks;

namespace DrugCheckingCrawler.Interface
{
    public interface IResourceCrawler
    {
        Task<ICrawlerResult> Crawl(int startIndex);
    }
}
