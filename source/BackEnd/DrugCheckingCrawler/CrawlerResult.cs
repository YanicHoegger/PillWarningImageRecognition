using DrugCheckingCrawler.Interface;
using System.Collections.Generic;

namespace DrugCheckingCrawler
{
    public class CrawlerResult : ICrawlerResult
    {
        public CrawlerResult(IList<CrawlerResultItem> items, int lastSuccessfullIndex)
        {
            Items = items;
            LastSuccessfullIndex = lastSuccessfullIndex;
        }

        public IEnumerable<ICrawlerResultItem> Items { get; }
        public int LastSuccessfullIndex { get; }
    }
}
