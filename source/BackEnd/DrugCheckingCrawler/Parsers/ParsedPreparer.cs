using System;
using System.Globalization;

namespace DrugCheckingCrawler.Parsers
{
    public class ParsedPreparer
    {
        private readonly TextParser _textParser;

        public ParsedPreparer(TextParser textParser)
        {
            _textParser = textParser;
        }

        public bool Success { get; private set; }

        public string Name { get; private set; }
        public DateTime Tested { get; private set; }
        public string TestedAsString => _textParser.Date;
        public string GeneralInfo { get; private set; }

        public void Prepare()
        {
            PrepareMiscellaneous();

            if (!Success)
                return;
        }

        private void PrepareMiscellaneous()
        {
            try
            {
                Tested = DateTime.Parse(_textParser.Date, CultureInfo.GetCultureInfo("de-CH"));
                Name = _textParser.Header.Replace("Warnung: ", string.Empty);

                GeneralInfo = GeneralInfoPreparer.Prepare(_textParser.GeneralInfo);
                Success = true;
            }
            catch (Exception)
            {
                //TODO: Log when no success
                Success = false;
            }
        }
    }
}
