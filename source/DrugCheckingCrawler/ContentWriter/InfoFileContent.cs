using System;
using System.Collections.Generic;

namespace DrugCheckingCrawler.ContentWriter
{
    public class InfoFileContent
    {
        public InfoFileContent(ParserResult parserResult, string webUrl, string id)
        {
            WebUrl = webUrl;
            Id = id;
            Name = parserResult.Name;
            Colors = parserResult.Colors;
            Tested = parserResult.Tested;
        }

        public string Name { get; }
        public IEnumerable<string> Colors { get; }
        public DateTime Tested { get; }
        public string WebUrl { get; }
        public string Id { get; }
    }
}
