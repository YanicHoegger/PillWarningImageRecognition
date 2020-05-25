using System;

namespace DrugCheckingCrawler.Parsers
{
    public class ParserResult
    {
        public ParserResult(string name, DateTime tested, byte[] image)
        {
            Name = name;
            Tested = tested;
            Image = image;
        }

        public string Name { get; }
        public DateTime Tested { get; }
        public byte[] Image { get; }
    }
}
