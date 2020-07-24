using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using DatabaseInteraction.Interface;
using Domain.Interface;

namespace Domain.Prediction
{
    public class PillWarning : IPillWarning
    {
        public PillWarning(DrugCheckingSource drugCheckingSource)
        {
            Header = drugCheckingSource.Header;
            Name = drugCheckingSource.Name;
            Color = drugCheckingSource.Color;
            Creation = drugCheckingSource.Creation;
            GeneralInfos = drugCheckingSource.GeneralInfos;
            RiskEstimationTitle = drugCheckingSource.RiskEstimationTitle;
            RiskEstimation = drugCheckingSource.RiskEstimation;

            Infos = drugCheckingSource
                .Infos
                .Select(x => new PillWarningInfo(x.Title, x.Info));

            SaferUseRulesTitle = drugCheckingSource.SaferUseRulesTitle;
            SaferUseRules = drugCheckingSource.SaferUseRules;
            PdfLocation = drugCheckingSource.PdfLocation;
            Image = drugCheckingSource.Image;
        }

        public string Header { get; }
        public string Name { get; }
        public Color Color { get; }
        public DateTime Creation { get; }
        public Dictionary<string, string> GeneralInfos { get; }
        public string RiskEstimationTitle { get; }
        public string RiskEstimation { get; }
        public IEnumerable<IPillWarningInfo> Infos { get; }
        public string SaferUseRulesTitle { get; }
        public IEnumerable<string> SaferUseRules { get; }
        public string PdfLocation { get; }
        public byte[] Image { get; }
    }
}
