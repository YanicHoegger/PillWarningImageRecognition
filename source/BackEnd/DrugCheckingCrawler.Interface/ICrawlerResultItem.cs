using System;

namespace DrugCheckingCrawler.Interface
{
    public interface ICrawlerResultItem
    {
        public string Name { get; }
        public DateTime Tested { get; }
        public byte[] Image { get; }
        string Url { get; }
        string DocumentHash { get; }
    }
}
