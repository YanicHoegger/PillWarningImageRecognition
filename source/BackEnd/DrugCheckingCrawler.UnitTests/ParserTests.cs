using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Telerik.JustMock;

namespace DrugCheckingCrawler.UnitTests
{
    [TestFixture]
    public class ParserTests
    {
        public static IEnumerable<TestCaseData> ParserInfoTestCaseData = new TestCaseData[]
        {
            new TestCaseData("Delfin.pdf", "Delfin", DateTime.Parse("August 2012", CultureInfo.GetCultureInfo("de-CH"))),
            new TestCaseData("Kreuz.pdf", "Kreuz", DateTime.Parse("Mai 2012", CultureInfo.GetCultureInfo("de-CH"))),
            new TestCaseData("No1.pdf", "No. 1", DateTime.Parse("Juni 2012", CultureInfo.GetCultureInfo("de-CH"))),
            new TestCaseData("Smiley.pdf", "Smiley", DateTime.Parse("September 2015", CultureInfo.GetCultureInfo("de-CH"))),
            new TestCaseData("NoName.pdf", "no name", DateTime.Parse("Juli 2019", CultureInfo.GetCultureInfo("de-CH")))
        };

        [TestCaseSource(nameof(ParserInfoTestCaseData))]
        public void ParserInfoTest(string fileName, string expectedName, DateTime expectedCreation)
        {
            GivenFileContent(fileName);
            WhenParse();
            ThenCorrectInfoParsed(expectedName, expectedCreation);
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
            var loggerMock = Mock.Create<ILogger<Parser>>();
            parserResult = new Parser(loggerMock).ParseFile(fileContent);
        }

        private void ThenCorrectInfoParsed(string expectedName, DateTime expectedCreation)
        {
            Assert.AreEqual(expectedName, parserResult.Name);
            Assert.AreEqual(expectedCreation, parserResult.Tested);
        }

        private void ThenResultNull()
        {
            Assert.IsNull(parserResult);
        }
    }
}
