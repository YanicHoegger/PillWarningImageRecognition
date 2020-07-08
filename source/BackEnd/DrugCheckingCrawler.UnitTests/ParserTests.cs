using DrugCheckingCrawler.Parsers;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Telerik.JustMock;

namespace DrugCheckingCrawler.UnitTests
{
    [TestFixture]
    public class ParserTests
    {
        [TestCaseSource(typeof(ParserTestsData), nameof(ParserTestsData.ParserInfoTestCaseData))]
        public void ParserInfoTest(string fileName, string expectedName, DateTime expectedCreation)
        {
            GivenFileContent(fileName);
            WhenParse();
            ThenCorrectInfoParsed(expectedName, expectedCreation);
        }

        [TestCaseSource(typeof(ParserTestsData), nameof(ParserTestsData.ParserRiskEstimationTestCaseData))]
        public void ParserRiskEstimationTest(string fileName, RiskEstimationContent expectedRiskEstimation)
        {
            GivenFileContent(fileName);
            WhenParse();
            ThenCorrectRiskEstimationParsed(expectedRiskEstimation);
        }

        [TestCaseSource(typeof(ParserTestsData), nameof(ParserTestsData.ParserInfosTestCaseData))]
        public void ParserInfosTest(string fileName, IEnumerable<InfoContent> expectedInfos)
        {
            GivenFileContent(fileName);
            WhenParse();
            ThenCorrectInfosParsed(expectedInfos);
        }

        [TestCaseSource(typeof(ParserTestsData), nameof(ParserTestsData.ParserSaferUseRulesTestCaseData))]
        public void ParserSaferUserRulesTest(string fileName, SaferUseRules saferUseRules)
        {
            GivenFileContent(fileName);
            WhenParse();
            ThenCorrectSaferUsrRules(saferUseRules);
        }

        [Test]
        public void ParseEmptyContentTest()
        {
            GivenFileContent("Empty.pdf");
            WhenParse();
            ThenResultNull();
        }

        private byte[] _fileContent;
        private ParserResult _parserResult;

        private void GivenFileContent(string fileName)
        {
            var filePath = TestHelper.GetAbsolutePath(fileName);
            _fileContent = File.ReadAllBytes(filePath);
        }

        private void WhenParse()
        {
            var loggerMock = Mock.Create<ILogger<Parser>>();
            _parserResult = new Parser(loggerMock).ParseFile(_fileContent);
        }

        private void ThenCorrectInfoParsed(string expectedName, DateTime expectedCreation)
        {
            Assert.AreEqual(expectedName, _parserResult.Parsed.Name);
            Assert.AreEqual(expectedCreation, _parserResult.Parsed.Tested);
        }

        private void ThenResultNull()
        {
            Assert.IsNull(_parserResult);
        }

        private void ThenCorrectRiskEstimationParsed(RiskEstimationContent expectedRiskEstimation)
        {
            Assert.AreEqual(expectedRiskEstimation.Title, _parserResult.Parsed.RiskEstimation.Title);
            Assert.AreEqual(expectedRiskEstimation.RiskEstimation, _parserResult.Parsed.RiskEstimation.RiskEstimation);
        }

        private void ThenCorrectInfosParsed(IEnumerable<InfoContent> expectedInfos)
        {
            var expectedArray = expectedInfos.ToArray();
            var actualArray = _parserResult.Parsed.Infos.ToArray();

            Assert.AreEqual(expectedArray.Length, actualArray.Length);

            for (var i = 0; i < expectedArray.Length; i++)
            {
                var actual = actualArray.Single(x => x.Title.Equals(expectedArray[i].Title));

                Assert.AreEqual(expectedArray[i].Info, actual.Info);
            }
        }

        private void ThenCorrectSaferUsrRules(SaferUseRules expectedSaferUseRules)
        {
            Assert.AreEqual(expectedSaferUseRules.Title, _parserResult.Parsed.SaferUseRules.Title);
            CollectionAssert.AreEqual(expectedSaferUseRules.Rules, _parserResult.Parsed.SaferUseRules.Rules);
        }
    }
}
