using System;
using System.Collections.Generic;

namespace DrugCheckingCrawler
{
    public class ParserResult
    {
        public ParserResult(string name, IEnumerable<string> colors, DateTime tested, byte[] image)
        {
            Name = name;
            Colors = colors;
            Tested = tested;
            Image = image;
        }

        public string Name { get; }
        public IEnumerable<string> Colors { get; }
        public DateTime Tested { get; }
        public byte[] Image { get; }
    }
}
