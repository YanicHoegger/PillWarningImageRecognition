using System;
using System.Collections.Generic;
using System.Drawing;

namespace DatabaseInteraction.Interface
{
    public interface IDrugCheckingSource : IEntity
    {
        string Header { get; set; }
        string Name { get; set; }

        Color Color { get; set; }

        DateTime Creation { get; set; }

        Dictionary<string, string> GeneralInfos { get; set; }

        string RiskEstimationTitle { get; set; }
        string RiskEstimation { get; set; }

        IEnumerable<IDrugCheckingInfo> Infos { get; set; }

        string SaferUseRulesTitle { get; set; }
        IEnumerable<string> SaferUseRules { get; set; }

        string PdfLocation { get; set; }

        byte[] Image { get; set; }

        string DocumentHash { get; set; }
    }
}
