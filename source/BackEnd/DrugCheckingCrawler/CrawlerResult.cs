using DrugCheckingCrawler.Interface;
using System.Collections.Generic;

namespace DrugCheckingCrawler
{
    public class CrawlerResult : ICrawlerResult
    {
        public CrawlerResult(IEnumerable<CrawlerResultItem> items, int lastSuccessfulIndex)
        {
            Items = items;
            LastSuccessfulIndex = lastSuccessfulIndex;
        }

        public IEnumerable<ICrawlerResultItem> Items { get; }
        public int LastSuccessfulIndex { get; }
    }
}
