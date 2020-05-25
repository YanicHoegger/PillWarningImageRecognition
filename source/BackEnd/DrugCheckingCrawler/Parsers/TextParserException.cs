using System;

namespace DrugCheckingCrawler.Parsers
{
    public class TextParserException : Exception
    {
        public TextParserException(string text)
            : base($"Something with this text is not right\r\n{text}")
        {
        }
    }
}
