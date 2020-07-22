using CustomVisionInteraction.Interface;
using System;
using System.Drawing;
using System.IO;
using Utilities;

namespace CustomVisionInteraction.ColorAnalyzer
{
    public class CroppingService : ICroppingService
    {
        public byte[] CropImage(byte[] image, IBoundingBox boundingBox)
        {
            using var streamOriginal = new MemoryStream(image);
            using var originalImage = Image.FromStream(streamOriginal) as Bitmap;

            var croppingRectangle = CreateCroppingRectangle(boundingBox, originalImage);

            using var target = new Bitmap(croppingRectangle.Width, croppingRectangle.Height);
            using (var graphics = Graphics.FromImage(target))
            {
                graphics.DrawImage(originalImage, new Rectangle(0, 0, target.Width, target.Height),
                                 croppingRectangle,
                                 GraphicsUnit.Pixel);
            }

            return target.ToArray();
        }

        private static Rectangle CreateCroppingRectangle(IBoundingBox boundingBox, Image originalImage)
        {
            var width = ToPixel(originalImage.Width * boundingBox.Width);
            var height = ToPixel(originalImage.Height * boundingBox.Height);
            var left = ToPixel(originalImage.Width * boundingBox.Left);
            var top = ToPixel(originalImage.Height * boundingBox.Top);

            return new Rectangle(left, top, width, height);
        }

        private static int ToPixel(double value)
        {
            return (int)Math.Round(value);
        }
    }
}
