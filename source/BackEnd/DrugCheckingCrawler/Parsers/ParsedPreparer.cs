using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DrugCheckingCrawler.Parsers
{
    public class ParsedPreparer
    {
        private const string _nameIdentifyer = "Name";
        private readonly TextParser _textParser;

        public ParsedPreparer(TextParser textParser)
        {
            _textParser = textParser;
        }

        public bool Success { get; private set; }

        public string Header { get; private set; }
        public string Name { get; private set; }
        public DateTime Tested { get; private set; }
        public Dictionary<string, string> GeneralInfo { get; private set; }

        public void Prepare()
        {
            PrepareMiscellaneous();

            if (!Success)
                return;
        }

        private void PrepareMiscellaneous()
        {
            Tested = DateTime.Parse(_textParser.Date, CultureInfo.GetCultureInfo("de-CH"));
            Header = _textParser.Header.Replace("Warnung: ", string.Empty);

            GeneralInfo = GeneralInfoPreparer
                .Prepare(_textParser.GeneralInfo)
                .ToDictionary(x => x.name, x => x.value);

            if (GeneralInfo.ContainsKey(_nameIdentifyer))
            {
                Name = GeneralInfo[_nameIdentifyer];
                Success = true;
            }
        }
    }
}
