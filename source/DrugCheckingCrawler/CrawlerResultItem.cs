namespace DrugCheckingCrawler
{
    public class CrawlerResultItem
    {
        public CrawlerResultItem(ParserResult parserResult, string url, string documentHash)
        {
            ParserResult = parserResult;
            Url = url;
            DocumentHash = documentHash;
        }

        public ParserResult ParserResult { get; }
        public string Url { get; }
        public string DocumentHash { get; }
    }
}
