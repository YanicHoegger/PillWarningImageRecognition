namespace DrugCheckingCrawler.Parsers
{
    public class TextItem
    {
        public TextItem(string titel, string content)
        {
            Titel = titel;
            Content = content;
        }

        public string Titel { get; }
        public string Content { get; }
    }
}
