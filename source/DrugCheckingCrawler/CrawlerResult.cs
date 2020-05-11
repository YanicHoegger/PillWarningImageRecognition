using System.Collections.Generic;

namespace DrugCheckingCrawler
{
    public class CrawlerResult
    {
        public CrawlerResult(IList<CrawlerResultItem> items, int lastSuccessfullIndex)
        {
            Items = items;
            LastSuccessfullIndex = lastSuccessfullIndex;
        }

        public IList<CrawlerResultItem> Items { get; }
        public int LastSuccessfullIndex { get; }
    }
}
