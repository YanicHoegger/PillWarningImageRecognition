using System;

namespace DrugCheckingCrawler.Parsers
{
    public class GeneralInfoPreparerException : Exception
    {
        public GeneralInfoPreparerException(string name)
             : base($"Could not find {name} in general info")
        {
        }
    }
}
