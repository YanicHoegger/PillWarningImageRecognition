using System;

namespace DatabaseInteraction
{
    public class CrawlerAction : Entity
    {
        public DateTime Executed { get; set; }

        public int LastSuccessfullIndex { get; set; }

        public int CrawlingCount { get; set; }
    }
}
