using System;

namespace DatabaseInteraction.Interface
{
    public class CrawlerAction : Entity
    {
        public DateTime Executed { get; set; }

        public int LastSuccessfulIndex { get; set; }

        public int CrawlingCount { get; set; }
    }
}
