using System;

namespace DrugCheckingCrawler.Parsers
{
    public class ParserResult
    {
        public ParserResult(DetailParser parsed, byte[] image)
        {
            Image = image;
            Parsed = parsed;
        }

        public DetailParser Parsed { get; }
        public byte[] Image { get; }
    }
}
