using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DrugCheckingCrawler.Parsers
{
    public static class GeneralInfoParser
    {
        private const string _generalInfoSeperator = "\t\t\t";

        //TODO: Make non static
        public static IEnumerable<(string name, string value)> Parse(string toPrepare)
        {
            var prePrepared = ParseInternal(toPrepare);

            var stringReader = new StringReader(prePrepared);
            string line;
            while ((line = stringReader.ReadLine()) != null)
            {
                var splitted = line.Split(_generalInfoSeperator);
                if(splitted.Length == 2)
                {
                    yield return (splitted[0], splitted[1]);
                }
                //TODO: Log incomplete general info
            }
        }

        private static string ParseInternal(string toPrepare)
        {
            var stringReader = new StringReader(toPrepare);
            var result = toPrepare;

            string line;
            var lastknownGeneralInfo = string.Empty;
            while((line = stringReader.ReadLine()) != null)
            {
                var knownGeneralInfo = KnownGeneralInfos
                    .List
                    .Where(x => line.Contains(x))
                    //If an element of the list contains an other, then more then one option would be possible.
                    //So we take the longer.
                    //E.g.: 'Inhaltsstoff' and 'Inhaltsstoffe'
                    .OrderByDescending(x => x.Length)
                    .FirstOrDefault();

                if (knownGeneralInfo == null)
                {
                    if(KnownGeneralInfos.PossibleLongContents.Contains(lastknownGeneralInfo))
                    {
                        var position = result.IndexOf(ParserConstants.NewLine, result.IndexOf(lastknownGeneralInfo));

                        result = result.Replace(ParserConstants.NewLine, " ", position);

                        continue;
                    }
                    else
                    {
                        throw new GeneralInfoParserException(line);
                    }
                }

                lastknownGeneralInfo = knownGeneralInfo;

                var knownGeneralInfoWithSpace = $"{knownGeneralInfo} ";

                if(line.Contains(knownGeneralInfoWithSpace) && line.Length > knownGeneralInfoWithSpace.Length)
                {
                    result = result.Replace($"{knownGeneralInfo} ", $"{knownGeneralInfo}{_generalInfoSeperator}");
                    continue;
                }

                //TODO: Log incomplete general info
            }

            return result;
        }
    }
}
