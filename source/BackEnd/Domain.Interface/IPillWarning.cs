using System;
using System.Collections.Generic;
using System.Drawing;

namespace Domain.Interface
{
    public interface IPillWarning
    {
        string Header { get; }
        string Name { get; }

        Color Color { get; }

        DateTime Creation { get; }

        Dictionary<string, string> GeneralInfos { get; }

        string RiskEstimationTitle { get; }
        string RiskEstimation { get; }

        IEnumerable<IPillWarningInfo> Infos { get; }

        string SaferUseRulesTitle { get; }
        IEnumerable<string> SaferUseRules { get; }

        string PdfLocation { get; }

        byte[] Image { get; }
    }
}
