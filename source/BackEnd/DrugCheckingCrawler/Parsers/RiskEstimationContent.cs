using DrugCheckingCrawler.Interface;

namespace DrugCheckingCrawler.Parsers
{
    public class RiskEstimationContent : IRiskEstimationContent
    {
        public RiskEstimationContent(string title, string content)
        {
            Title = title;
            RiskEstimation = content;
        }

        public string Title { get; }
        public string RiskEstimation { get; }
    }
}
