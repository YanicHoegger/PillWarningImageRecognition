using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace DrugCheckingCrawler.UnitTests
{
    [TestFixture]
    public class ParserTests
    {
        public static IEnumerable<TestCaseData> ParserInfoTestCaseData = new TestCaseData[]
        {
            new TestCaseData("Delfin.pdf", "Delfin", new string[] { "grün", "gelb" }, DateTime.Parse("August 2012", CultureInfo.GetCultureInfo("de-CH"))),
            new TestCaseData("Kreuz.pdf", "Kreuz", new string[] { "braun" }, DateTime.Parse("Mai 2012", CultureInfo.GetCultureInfo("de-CH"))),
            new TestCaseData("No1.pdf", "No. 1", new string[] { "grün" }, DateTime.Parse("Juni 2012", CultureInfo.GetCultureInfo("de-CH"))),
            new TestCaseData("Smiley.pdf", "Smiley", new string[] { "blau" }, DateTime.Parse("September 2015", CultureInfo.GetCultureInfo("de-CH"))),
            new TestCaseData("NoName.pdf", "no name", new string[] { "blau" }, DateTime.Parse("Juli 2019", CultureInfo.GetCultureInfo("de-CH")))
        };

        [TestCaseSource(nameof(ParserInfoTestCaseData))]
        public void ParserInfoTest(string fileName, string expectedName, IEnumerable<string> expectedColors, DateTime expectedCreation)
        {
            GivenFileContent(fileName);
            WhenParse();
            ThenCorrectInfoParsed(expectedName, expectedColors, expectedCreation);
        }

        [Test]
        public void ParseEmptyContentTest()
        {
            GivenFileContent("Empty.pdf");
            WhenParse();
            ThenResultNull();
        }

        private byte[] fileContent;
        private ParserResult parserResult;

        private void GivenFileContent(string fileName)
        {
            var filePath = Path.Combine(TestContext.CurrentContext.WorkDirectory, "TestFiles", fileName);
            fileContent = File.ReadAllBytes(filePath);
        }

        private void WhenParse()
        {
            parserResult = new Parser().ParseFile(fileContent);
        }

        private void ThenCorrectInfoParsed(string expectedName, IEnumerable<string> expectedColors, DateTime expectedCreation)
        {
            Assert.AreEqual(expectedName, parserResult.Name);
            Assert.AreEqual(expectedColors, parserResult.Colors);
            Assert.AreEqual(expectedCreation, parserResult.Tested);
        }

        private void ThenResultNull()
        {
            Assert.IsNull(parserResult);
        }
    }
}
