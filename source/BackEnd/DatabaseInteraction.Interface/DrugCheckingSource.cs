using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.Json.Serialization;
using Utilities;

namespace DatabaseInteraction.Interface
{
    public class DrugCheckingSource : Entity
    {
        public string Name { get; set; }

        [JsonConverter(typeof(ColorSerializer))]
        public Color Color { get; set; }

        public DateTime Creation { get; set; }

        public string RiskEstimationTitle { get; set; }
        public string RiskEstimation { get; set; }

        //TODO: One should not use tuples in interface assemblies
        public List<(string title, string content)> Infos { get; set; }

        public string SaferUseRulesTitle { get; set; }
        public List<string> SaferUseRules { get; set; }

        public string PdfLocation { get; set; }

        public byte[] Image { get; set; }

        public string DocumentHash { get; set; }
    }
}
