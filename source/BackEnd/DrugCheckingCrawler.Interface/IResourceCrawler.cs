namespace DrugCheckingCrawler.Interface
{
    public interface IResourceCrawler
    {
        ICrawlerResult Crawl(int startIndex);
    }
}
