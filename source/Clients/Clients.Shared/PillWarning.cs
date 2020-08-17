using System.Collections.Generic;
using System.Drawing;
using System.Text.Json.Serialization;
using Utilities;

namespace Clients.Shared
{
    public class PillWarning
    {
        public string Header { get; set; }
        public string Name { get; set; }

        [JsonConverter(typeof(ColorSerializer))]
        public Color Color { get; set; }

        public Dictionary<string, string> GeneralInfos { get; set; }

        public string RiskEstimationTitle { get; set; }
        public string RiskEstimation { get; set; }

        public List<PillWarningInfo> Infos { get; set; }

        public string SaferUseRulesTitle { get; set; }
        public List<string> SaferUseRules { get; set; }

        public byte[] Image { get; set; }
    }
}
