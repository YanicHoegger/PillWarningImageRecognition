using System;
using System.Collections.Generic;
using System.Drawing;
using Domain.Interface;

namespace Domain.Mock
{
    public class PillWarningMock : IPillWarning
    {
        public string Header { get; set; }
        public string Name { get; set; }
        public Color Color { get; set; }
        public DateTime Creation { get; set; }
        public Dictionary<string, string> GeneralInfos { get; set; }
        public string RiskEstimationTitle { get; set; }
        public string RiskEstimation { get; set; }
        public IEnumerable<IPillWarningInfo> Infos { get; set; }
        public string SaferUseRulesTitle { get; set; }
        public IEnumerable<string> SaferUseRules { get; set; }
        public string PdfLocation { get; set; }
        public byte[] Image { get; set; }
    }
}
