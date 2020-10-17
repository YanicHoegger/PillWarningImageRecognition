using System.Collections.Generic;
using System.Threading.Tasks;

namespace DrugCheckingCrawler.Interface
{
    public interface ICrawlerResult
    {
        IAsyncEnumerable<ICrawlerResultItem> Items { get; }
        Task<int> LastSuccessfulIndex { get; }
    }
}
