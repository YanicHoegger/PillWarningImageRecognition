using System;
using System.Drawing;
using System.IO;
using FluentAssertions;
using ImageInteraction.Detection;
using ImageInteraction.ImageManipulation;
using NUnit.Framework;

namespace ImageInteraction.UnitTests
{
    [TestFixture]
    public class CroppingServiceTests
    {
        [Test]
        public void CroppingTest()
        {
            GivenImage();
            WhenCropped();
            ThenCropped();
        }

        private byte[] _original;
        private byte[] _cropped;

        private void GivenImage()
        {
            _original = TestHelper.ReadImage("Skulls1.jpg");
        }

        private void WhenCropped()
        {
            _cropped = new CroppingService().CropImage(_original, new BoundingBox(0.25, 0.25, 0.5, 0.5));
        }

        private void ThenCropped()
        {
            var actual = Image.FromStream(new MemoryStream(_cropped));
            var original = Image.FromStream(new MemoryStream(_original));

            actual.Width.Should().Be((int)Math.Round(original.Width * 0.5));
            actual.Height.Should().Be((int)Math.Round(original.Height * 0.5));
        }
    }
}
