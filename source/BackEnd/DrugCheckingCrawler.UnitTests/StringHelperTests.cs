using DrugCheckingCrawler.Parsers;
using NUnit.Framework;
using System;

namespace DrugCheckingCrawler.UnitTests
{
    [TestFixture]
    public class StringHelperTests
    {
        [Test]
        public void RemoveSpacesBeforeTest()
        {
            GivenStringWithSpaces();
            WhenRemoveSpaces();
            ThenCorrectString();
        }

        [Test]
        public void NoSpacesTest()
        {
            GivenStringNoSpaces();
            WhenRemoveSpaces();
            ThenCorrectString();
        }

        private string _teste;

        private void GivenStringWithSpaces()
        {
            _teste = "Hello   There";
        }

        private void GivenStringNoSpaces()
        {
            _teste = "HelloThere";
        }

        private void WhenRemoveSpaces()
        {
            _teste = _teste.RemoveAllBefore(' ', _teste.IndexOf("T"));
        }

        private void ThenCorrectString()
        {
            Assert.AreEqual("HelloThere", _teste);
        }
    }
}
