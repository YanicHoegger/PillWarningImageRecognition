using DrugCheckingCrawler.Parsers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace DrugCheckingCrawler.UnitTests
{
    public static class ParserTestsData
    {
        private static readonly string _deflinPdfPath = Path.Combine("Delfin", "Delfin.pdf");
        private static readonly string _kreuzPdfPath = Path.Combine("Kreuz", "Kreuz.pdf");
        private static readonly string _no1PdfPath = Path.Combine("No1", "No1.pdf");
        private static readonly string _smileyPdfPath = Path.Combine("Smiley", "Smiley.pdf");
        private static readonly string _noNamePdfPath = Path.Combine("NoName", "NoName.pdf");

        public static readonly IEnumerable<TestCaseData> ParserInfoTestCaseData = new TestCaseData[]
        {
            new TestCaseData(_deflinPdfPath, "Delfin", DateTime.Parse("August 2012", CultureInfo.GetCultureInfo("de-CH"))),
            new TestCaseData(_kreuzPdfPath, "Kreuz", DateTime.Parse("Mai 2012", CultureInfo.GetCultureInfo("de-CH"))),
            new TestCaseData(_no1PdfPath, "No. 1", DateTime.Parse("Juni 2012", CultureInfo.GetCultureInfo("de-CH"))),
            new TestCaseData(_smileyPdfPath, "Smiley", DateTime.Parse("September 2015", CultureInfo.GetCultureInfo("de-CH"))),
            new TestCaseData(_noNamePdfPath, "no name", DateTime.Parse("Juli 2019", CultureInfo.GetCultureInfo("de-CH")))
        };

        public static readonly IEnumerable<TestCaseData> ParserRiskEstimationTestCaseData = new TestCaseData[]
        {
            new TestCaseData(_deflinPdfPath, ReadRiskEstimation("Delfin")),
            new TestCaseData(_kreuzPdfPath, ReadRiskEstimation("Kreuz")),
            new TestCaseData(_no1PdfPath, ReadRiskEstimation("No1")),
            new TestCaseData(_smileyPdfPath, ReadRiskEstimation("Smiley")),
            new TestCaseData(_noNamePdfPath, ReadRiskEstimation("NoName"))
        };

        public static readonly IEnumerable<TestCaseData> ParserInfosTestCaseData = new TestCaseData[]
        {
            new TestCaseData(_deflinPdfPath, ReadInfos("Delfin")),
            new TestCaseData(_kreuzPdfPath, ReadInfos("Kreuz")),
            new TestCaseData(_no1PdfPath, ReadInfos("No1")),
            new TestCaseData(_smileyPdfPath, ReadInfos("Smiley")),
            new TestCaseData(_noNamePdfPath, ReadInfos("NoName"))
        };

        public static readonly IEnumerable<TestCaseData> ParserSaferUseRulesTestCaseData = new TestCaseData[]
        {
            new TestCaseData(_deflinPdfPath, GetSaferUseRules("Delfin")),
            new TestCaseData(_kreuzPdfPath, GetSaferUseRules("Kreuz")),
            new TestCaseData(_no1PdfPath, GetSaferUseRules("No1")),
            new TestCaseData(_smileyPdfPath, GetSaferUseRules("Smiley")),
            new TestCaseData(_noNamePdfPath, GetSaferUseRules("NoName"))
        };

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
