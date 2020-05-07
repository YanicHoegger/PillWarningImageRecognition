using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models;
using System.IO;
using System.Drawing;
using System;
using Utilities;

namespace CustomVisionInteraction.Prediction
{
    public class CroppingService : ICroppingService
    {
        public byte[] CropImage(byte[] image, BoundingBox boundingBox)
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

        private static Rectangle CreateCroppingRectangle(BoundingBox boundingBox, Image originalImage)
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
