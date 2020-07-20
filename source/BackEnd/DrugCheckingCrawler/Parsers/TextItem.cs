namespace DrugCheckingCrawler.Parsers
{
    public class TextItem
    {
        public TextItem(string titel, string content)
        {
            Title = titel;
            Content = content;
        }

        public string Title { get; }
        public string Content { get; }
    }
}
