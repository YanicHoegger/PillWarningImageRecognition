using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.Json.Serialization;
using DatabaseInteraction.Interface;
using Utilities;

namespace DatabaseInteraction
{
    public class DrugCheckingSource : Entity.Entity, IDrugCheckingSource
    {
        public string Header { get; set; }
        public string Name { get; set; }

        [JsonConverter(typeof(ColorSerializer))]
        public Color Color { get; set; }

        public DateTime Creation { get; set; }

        public Dictionary<string, string> GeneralInfos { get; set; }

        public string RiskEstimationTitle { get; set; }
        public string RiskEstimation { get; set; }

        public IEnumerable<IDrugCheckingInfo> Infos { get; set; }

        public string SaferUseRulesTitle { get; set; }
        public IEnumerable<string> SaferUseRules { get; set; }

        public string PdfLocation { get; set; }

        public byte[] Image { get; set; }

        public string DocumentHash { get; set; }
    }
}
