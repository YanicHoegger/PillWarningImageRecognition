using DrugCheckingCrawler.Interface;

namespace DrugCheckingCrawler.Parsers
{
    public class InfoContent : IInfoContent
    {
        public InfoContent(string title, string info)
        {
            Title = title;
            Info = info;
        }

        public string Title { get; }
        public string Info { get; }
    }
}
