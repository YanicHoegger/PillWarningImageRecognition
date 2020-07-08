using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace DrugCheckingCrawler.Parsers
{
    public class DetailParser
    {
        private const string _nameIdentifyer = "Name";

        private const string _subTitleName = "Titel";
        private static readonly Regex _subTitleRegex = new Regex($"\n(?<{_subTitleName}>[\\w ]+): ");

        private readonly SectionParser _sectionParser;

        public DetailParser(SectionParser sectionParser)
        {
            _sectionParser = sectionParser;
        }

        public bool Success { get; private set; }

        public string Header { get; private set; }
        public string Name { get; private set; }
        public DateTime Tested { get; private set; }
        public Dictionary<string, string> GeneralInfo { get; private set; }

        public RiskEstimationContent RiskEstimation { get; private set; }
        public IEnumerable<InfoContent> Infos { get; private set; }
        public SaferUseRules SaferUseRules { get; private set; }

        public void Parse()
        {
            PrepareMiscellaneous();

            if (!Success)
                return;

            PrepareRiskEstimation();
            PrepareInfos();
            PrepareSaferUseRules();
        }

        private void PrepareMiscellaneous()
        {
            Tested = DateTime.Parse(_sectionParser.Date, CultureInfo.GetCultureInfo("de-CH"));
            Header = _sectionParser.Header.Replace("Warnung: ", string.Empty);

            GeneralInfo = GeneralInfoParser
                .Parse(_sectionParser.GeneralInfo)
                .ToDictionary(x => x.name, x => x.value);

            if (GeneralInfo.ContainsKey(_nameIdentifyer))
            {
                Name = GeneralInfo[_nameIdentifyer];
                Success = true;
            }
        }

        private void PrepareRiskEstimation()
        {
            var content = StripTextOfNewLines(_sectionParser.RiskEstimation.Content);
            RiskEstimation = new RiskEstimationContent(_sectionParser.RiskEstimation.Title, content);
        }

        private void PrepareInfos()
        {
            Infos = _sectionParser.Infos.Select(ParseInfo).ToList();
        }

        private InfoContent ParseInfo(TextItem textItem)
        {
            //TODO: Move to own class
            var content = textItem.Content;
            var matches = _subTitleRegex.Matches(content);

            foreach(Match match in matches)
            {
                var position = content.IndexOf($"{match.Groups[_subTitleName].Value}:") - 1;
                content = content.Replace(ParserConstants.NewLine, "------", position)
                    //Sometimes a space is at the end of a line, we make sure here these get stripped to
                    .RemoveAllBefore(' ', position);
            }

            var stripped = StripTextOfNewLines(content);
            var parsedContent = stripped.Replace("------", Environment.NewLine);

            return new InfoContent(textItem.Title, parsedContent);
        }

        private void PrepareSaferUseRules()
        {
            var rules = _sectionParser
                .SaferUseRules
                .Content
                .Split(" • ")
                .Skip(1)
                .Select(x => StripTextOfNewLines(x));

            SaferUseRules = new SaferUseRules(_sectionParser.SaferUseRules.Title, rules);
        }

        private static string StripTextOfNewLines(string text)
        {
            //Sometimes a space is at the end of a line, we make sure here these get stripped to
            var intermediateResult = text.Replace(" " + ParserConstants.NewLine, " ");
            return intermediateResult.Replace(ParserConstants.NewLine, " ").Trim();
        }
    }
}
