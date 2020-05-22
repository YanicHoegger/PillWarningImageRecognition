using System.Collections.Generic;

namespace DrugCheckingCrawler.Interface
{
    public interface ICrawlerResult
    {
        IEnumerable<ICrawlerResultItem> Items { get; }
        int LastSuccessfullIndex { get; }
    }
}
