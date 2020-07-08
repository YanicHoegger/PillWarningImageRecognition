using DrugCheckingCrawler.Interface;
using DrugCheckingCrawler.Parsers;
using System;
using System.Collections.Generic;

namespace DrugCheckingCrawler
{
    public class CrawlerResultItem : ICrawlerResultItem
    {
        public CrawlerResultItem(ParserResult parserResult, string url, string documentHash)
        {
            Header = parserResult.Parsed.Header;
            Name = parserResult.Parsed.Name;
            Tested = parserResult.Parsed.Tested;
            GeneralInfos = parserResult.Parsed.GeneralInfo;
            Image = parserResult.Image;

            RiskEstimation = parserResult.Parsed.RiskEstimation;
            Infos = parserResult.Parsed.Infos;
            SaferUserRules = parserResult.Parsed.SaferUseRules;

            Url = url;
            DocumentHash = documentHash;
        }

        public string Header { get; }
        public string Name { get; }
        public DateTime Tested { get; }
        public Dictionary<string, string> GeneralInfos { get; }
        public byte[] Image { get; }

        public IRiskEstimationContent RiskEstimation { get; }
        public IEnumerable<IInfoContent> Infos { get; }
        public ISaferUserRules SaferUserRules { get; }

        public string Url { get; }
        public string DocumentHash { get; }
    }
}
