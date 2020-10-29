using System;
using DatabaseInteraction.Interface;

namespace DatabaseInteraction
{
    public class CrawlerAction : Entity.Entity, ICrawlerAction
    {
        public DateTime Executed { get; set; }

        public int LastSuccessfulIndex { get; set; }

        public int CrawlingCount { get; set; }
    }
}
