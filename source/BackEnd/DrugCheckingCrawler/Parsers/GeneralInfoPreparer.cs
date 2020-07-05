using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DrugCheckingCrawler.Parsers
{
    public static class GeneralInfoPreparer
    {
        private const string _generalInfoSeperator = "\t\t\t";

        public static IEnumerable<(string name, string value)> Prepare(string toPrepare)
        {
            var prePrepared = PrepareInternal(toPrepare);

            var stringReader = new StringReader(prePrepared);
            string line;
            while ((line = stringReader.ReadLine()) != null)
            {
                var splitted = line.Split(_generalInfoSeperator);
                yield return (splitted[0], splitted[1]);
            }
        }

        private static string PrepareInternal(string toPrepare)
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
                        result = result.Remove(position, ParserConstants.NewLine.Length);
                        result = result.Insert(position, " ");

                        continue;
                    }
                    else
                    {
                        throw new GeneralInfoPreparerException(line);
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
