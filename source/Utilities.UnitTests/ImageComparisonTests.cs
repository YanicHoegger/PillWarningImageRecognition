using NUnit.Framework;

namespace Utilities.UnitTests
{
    public class ImageComparisonTests
    {
        [Test]
        public void CompareImagesTest()
        {
            GivenSameImages();
            WhenCompare();
            ThenEqual();
        }

        private byte[] _imageOne;
        private byte[] _imageTwo;

        private bool _result;

        private void GivenSameImages()
        {
            _imageOne = TestHelper.ReadImage("CustomVisionImage.jpg");
            _imageTwo = TestHelper.ReadImage("DataBaseImage.jpg");
        }

        private void WhenCompare()
        {
            _result = ImageHelper.Compare(_imageOne, _imageTwo);
        }

        private void ThenEqual()
        {
            if (!_result)
            {
                Assert.Fail("Images are not equal");
            }
        }
    }
}
