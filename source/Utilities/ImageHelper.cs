using System;
using System.Drawing;
using System.IO;

namespace Utilities
{
    public static class ImageHelper
    {
        private const double _minSuccessRate = 0.9;
        private const int _colorTolerance = 5;

        public static bool Compare(byte[] image1ByteArray, byte[] image2ByteArray)
        {
            var image1 = FromByteArray(image1ByteArray);
            var image2 = FromByteArray(image2ByteArray);

            if (!CompareSize(image1, image2))
                return false;

            var rights = 0;
            for (var x = 0; x < image1.Width; x++)
            {
                for (int y = 0; y < image1.Height; y++)
                {
                    var pixelImage1 = image1.GetPixel(x, y);
                    var pixelImage2 = image2.GetPixel(x, y);

                    if (CompareColor(pixelImage1, pixelImage2))
                    {
                        rights++;
                    }
                }
            }

            return (double)rights / (image1.Width * image1.Height) > _minSuccessRate;
        }

        public static Bitmap FromByteArray(byte[] image)
        {
            using var memoryStream = new MemoryStream(image);

            return new Bitmap(memoryStream);
        }

        private static bool CompareSize(Image image1, Image image2)
        {
            return image1.Height == image2.Height && image1.Width == image2.Width;
        }

        private static bool CompareColor(Color color1, Color color2)
        {
            return CompareColorComponent(color1.A, color2.A)
                   && CompareColorComponent(color1.R, color2.R)
                   && CompareColorComponent(color1.G, color2.G)
                   && CompareColorComponent(color1.B, color2.B);
        }

        private static bool CompareColorComponent(byte component1, byte component2)
        {
            return Math.Abs(component1 - component2) <= _colorTolerance;
        }
    }
}
