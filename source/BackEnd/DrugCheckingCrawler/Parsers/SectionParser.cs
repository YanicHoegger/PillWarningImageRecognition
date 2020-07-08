using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace DrugCheckingCrawler.Parsers
{
    public class SectionParser
    {
        private const string _infoTitleLiteral = "Info zu";
        private const string _riskEstimationLiteral = "Risikoeinschätzung";

        private readonly string _allText;

        private StringReader _stringReader;
        private string _rawHeader;

        public SectionParser(string allText)
        {
            _stringReader = new StringReader(allText);
            _allText = allText;
        }

        public bool Success { get; private set; }

        public string Header { get; private set; }
        public string Date { get; private set; }
        public string GeneralInfo { get; private set; }
        public TextItem RiskEstimation { get; private set; }
        public List<TextItem> Infos { get; } = new List<TextItem>();
        public TextItem SaferUseRules { get; private set; }

        public void Parse()
        {
            try
            {
                ReadMiscellaneous();
                PrepareRestOfText();
                ReadRiskEstimationAndSaferUseRules();
                Success = true;
            }
            catch (TextParserException)
            {
                Success = false; 
                //TODO: logging
            }
        }

        private void ReadMiscellaneous()
        {
            (_rawHeader, Date) = ReadTillFind(new Regex("[\\w]+ [0-9]{4,4}"));

            Header = _rawHeader.Replace(ParserConstants.NewLine, string.Empty);
            GeneralInfo = ReadTillFind(_riskEstimationLiteral).read;
        }

        private void PrepareRestOfText()
        {
            var leftText = _stringReader.ReadToEnd();
            var strippedText = leftText
                .Replace(_rawHeader, string.Empty)
                .Replace(Date + ParserConstants.NewLine, string.Empty);

            _stringReader = new StringReader(strippedText);
        }

        private void ReadRiskEstimationAndSaferUseRules()
        {
            var (riskEstimationContent, infoTitel) = ReadTillFind(_infoTitleLiteral);

            RiskEstimation = new TextItem(_riskEstimationLiteral, riskEstimationContent);

            while (HasMoreInfos())
            {
                var (read, foundLine) = ReadTillFind(_infoTitleLiteral);
                Infos.Add(new TextItem(infoTitel, read));
                infoTitel = foundLine;
            }

            var (lastInfoContent, saferUseTitel) = ReadTillFind("Safer Use Regeln");
            Infos.Add(new TextItem(infoTitel, lastInfoContent));

            SaferUseRules = new TextItem(saferUseTitel, _stringReader.ReadToEnd());
        }

        private (string read, string foundLine) ReadTillFind(string toFind)
        {
            return ReadTillFind(x => x.Contains(toFind));
        }

        private (string read, string foundLine) ReadTillFind(Regex regex)
        {
            return ReadTillFind(x => regex.Match(x).Success);
        }

        private (string read, string foundLine) ReadTillFind(Func<string, bool> predicate)
        {
            var stringBuilder = new StringBuilder();
            do
            {
                var readLine = _stringReader.ReadLine();

                if (readLine == null)
                    throw new TextParserException(_allText);

                if (predicate(readLine))
                {
                    return (stringBuilder.ToString(), readLine);
                }
                else
                {
                    //In the text we hve different new line than StringBuilder does
                    stringBuilder.Append(readLine + ParserConstants.NewLine);
                }
            }
            while (true);
        }

        private bool HasMoreInfos()
        {
            var leftText = _stringReader.ReadToEnd();
            _stringReader = new StringReader(leftText);

            return leftText.Contains(_infoTitleLiteral);
        }
    }
}
