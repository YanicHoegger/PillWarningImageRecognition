using DrugCheckingCrawler.Interface;
using System.Collections.Generic;

namespace DrugCheckingCrawler.Parsers
{
    public class SaferUseRules : ISaferUserRules
    {
        public SaferUseRules(string title, IEnumerable<string> rules)
        {
            Title = title;
            Rules = rules;
        }

        public string Title { get; }
        public IEnumerable<string> Rules { get; }
    }
}
