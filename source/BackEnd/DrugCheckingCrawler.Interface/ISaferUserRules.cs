using System.Collections.Generic;

namespace DrugCheckingCrawler.Interface
{
    public interface ISaferUserRules : IContentItem
    {
        IEnumerable<string> Rules { get; }
    }
}
