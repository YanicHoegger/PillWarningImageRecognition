using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Drawing;

namespace Utilities.UnitTests
{
    [TestFixture]
    public class ImageColorAnalyzerTests
    {
        public static IEnumerable<TestCaseData> TestCaseDatas = new TestCaseData[]
        {
            new TestCaseData("Blue.jpg", Color.Blue),
            new TestCaseData("Green.jpg", Color.Green),
            new TestCaseData("Orange.jpg", Color.Orange),
            new TestCaseData("Pink.jpg", Color.Pink),
            new TestCaseData("Yellow.jpg", Color.Yellow)
        };

        [TestCaseSource(nameof(TestCaseDatas))]
        public void RightColorTest(string fileName, Color expected)
        {
            GivenImage(fileName);
            WhenAnalyze();
            ThenRightColor(expected);
        }

        private byte[] _image;
        private Color _analyzed;

        private void GivenImage(string fileName)
        {
            _image = TestHelper.ReadImage(fileName);
        }

        private void WhenAnalyze()
        {
            _analyzed = new ImageColorAnalyzer().GetMainColor(_image);
        }

        private void ThenRightColor(Color expected)
        {
            _analyzed.Should().Be(expected);
        }
    }
}
