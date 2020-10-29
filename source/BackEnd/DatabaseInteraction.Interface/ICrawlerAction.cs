using System;

namespace DatabaseInteraction.Interface
{
    public interface ICrawlerAction : IEntity
    {
        DateTime Executed { get; set; }
        int LastSuccessfulIndex { get; set; }
        int CrawlingCount { get; set; }
    }
}
