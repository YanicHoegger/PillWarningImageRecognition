using System;

namespace DrugCheckingCrawler.Parsers
{
    public class GeneralInfoPreparerException : Exception
    {
        public GeneralInfoPreparerException(string line)
             : base(CreateExceptionMessage(line))
        {
        }

        private static string CreateExceptionMessage(string line)
        {
            string candidates = string.Join(", ", $"'{KnownGeneralInfos.List}'");
            return $"Could not find a general info in line '{line}'. Candidates are {candidates}";
        }
    }
}
