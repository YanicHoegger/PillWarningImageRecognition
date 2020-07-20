using System;
using System.Collections.Generic;

namespace DrugCheckingCrawler.Interface
{
    public interface ICrawlerResultItem
    {
        string Header { get; }
        string Name { get; }
        DateTime Tested { get; }
        Dictionary<string, string> GeneralInfos { get; }
        byte[] Image { get; }

        IRiskEstimationContent RiskEstimation { get; }
        IEnumerable<IInfoContent> Infos { get; }
        ISaferUserRules SaferUserRules { get; }

        string Url { get; }
        string DocumentHash { get; }
    }
}
