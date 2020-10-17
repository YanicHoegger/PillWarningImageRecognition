using DrugCheckingCrawler.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DrugCheckingCrawler
{
    public class CrawlerResult : ICrawlerResult
    {
        public CrawlerResult(IAsyncEnumerable<ICrawlerResultItem> items, Task<int> lastSuccessfulIndex)
        {
            Items = items;
            LastSuccessfulIndex = lastSuccessfulIndex;
        }

        public IAsyncEnumerable<ICrawlerResultItem> Items { get; }
        public Task<int> LastSuccessfulIndex { get; }
    }
}
