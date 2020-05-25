using DrugCheckingCrawler.Interface;
using DrugCheckingCrawler.Parsers;
using System;

namespace DrugCheckingCrawler
{
    public class CrawlerResultItem : ICrawlerResultItem
    {
        public CrawlerResultItem(ParserResult parserResult, string url, string documentHash)
        {
            Name = parserResult.Name;
            Tested = parserResult.Tested;
            Image = parserResult.Image;
            Url = url;
            DocumentHash = documentHash;
        }

        public string Name { get; }
        public DateTime Tested { get; }
        public byte[] Image { get; }
        public string Url { get; }
        public string DocumentHash { get; }
    }
}
