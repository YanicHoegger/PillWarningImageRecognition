using System;
using System.Collections.Generic;

namespace DrugCheckingCrawler.ContentWriter
{
    public class InfoFileContent
    {
        public InfoFileContent()
        {
        }

        public InfoFileContent(ParserResult parserResult, string webUrl, string id)
        {
            WebUrl = webUrl;
            Id = id;
            Name = parserResult.Name;
            Colors = parserResult.Colors;
            Tested = parserResult.Tested;
        }

        public string Name { get; set; }
        public IEnumerable<string> Colors { get; set; }
        public DateTime Tested { get; set; }
        public string WebUrl { get; set; }
        public string Id { get; set; }
    }
}
