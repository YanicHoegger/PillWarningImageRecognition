using DrugCheckingCrawler.Parsers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace DrugCheckingCrawler.UnitTests
{
    public static class ParserTestsData
    {
        private static readonly string _delfinDirectoryName = "Delfin";
        private static readonly string _kreuzDirectoryName = "Kreuz";
        private static readonly string _no1DirectoryName = "No1";
        private static readonly string _smileyDirectoryName = "Smiley";
        private static readonly string _noNameDirectoryName = "NoName";

        private static readonly IEnumerable<string> _allDirectoryNames = new List<string>
        {
            _delfinDirectoryName,
            _kreuzDirectoryName,
            _no1DirectoryName,
            _smileyDirectoryName,
            _noNameDirectoryName
        };

        private static readonly IEnumerable<(string directoryName, string pdfPath)> _baseTestData =
            _allDirectoryNames.Select(x => (x, GetPdfPath(x)));

        public static readonly IEnumerable<TestCaseData> ParserInfoTestCaseData = new TestCaseData[]
        {
            new TestCaseData(GetPdfPath(_delfinDirectoryName), "Delfin", DateTime.Parse("August 2012", CultureInfo.GetCultureInfo("de-CH"))),
            new TestCaseData(GetPdfPath(_kreuzDirectoryName), "Kreuz", DateTime.Parse("Mai 2012", CultureInfo.GetCultureInfo("de-CH"))),
            new TestCaseData(GetPdfPath(_no1DirectoryName), "No. 1", DateTime.Parse("Juni 2012", CultureInfo.GetCultureInfo("de-CH"))),
            new TestCaseData(GetPdfPath(_smileyDirectoryName), "Smiley", DateTime.Parse("September 2015", CultureInfo.GetCultureInfo("de-CH"))),
            new TestCaseData(GetPdfPath(_noNameDirectoryName), "no name", DateTime.Parse("Juli 2019", CultureInfo.GetCultureInfo("de-CH")))
        };

        public static readonly IEnumerable<TestCaseData> ParserRiskEstimationTestCaseData =
            _baseTestData.Select(x => new TestCaseData(x.pdfPath, ReadRiskEstimation(x.directoryName)));

        public static readonly IEnumerable<TestCaseData> ParserInfosTestCaseData =
            _baseTestData.Select(x => new TestCaseData(x.pdfPath, ReadInfos(x.directoryName)));

        public static readonly IEnumerable<TestCaseData> ParserSaferUseRulesTestCaseData =
            _baseTestData.Select(x => new TestCaseData(x.pdfPath, GetSaferUseRules(x.directoryName)));

        private static string GetPdfPath(string directoryName)
        {
            return Path.Combine(directoryName, $"{directoryName}.pdf");
        }

        private static RiskEstimationContent ReadRiskEstimation(string relativePath)
        {
            var content = ReadFileContent(Path.Combine(relativePath, "RiskEstimation.txt"));
            return new RiskEstimationContent("Risikoeinschätzung", content);
        }

        private static IEnumerable<InfoContent> ReadInfos(string directory)
        {
            var path = TestHelper.GetAbsolutePath(Path.Combine(directory, "Infos"));
            
            foreach(var file in Directory.GetFiles(path, "*.txt"))
            {
                yield return new InfoContent(Path.GetFileNameWithoutExtension(file), File.ReadAllText(file));
            }
        }

        private static string ReadFileContent(string relativePath)
        {
            var path = TestHelper.GetAbsolutePath(relativePath);
            var unprocessedContent = File.ReadAllText(path);

            return unprocessedContent.Replace(Environment.NewLine, " ");
        }

        private static SaferUseRules GetSaferUseRules(string directory)
        {
            var path = TestHelper.GetAbsolutePath(Path.Combine(directory, "SaferUseRules.txt"));
            return new SaferUseRules("Safer Use Regeln", File.ReadAllLines(path));
        }
    }
}
