using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Utilities
{
    public static class ImageHelper
    {
        public static Stream ReadImage(string path)
        {
            return Image.FromFile(path).ToStream();
        }

        public static byte[] ReadImageAsArray(string path)
        {
            return Image.FromFile(path).ToArray();
        }

        public static byte[] ToArray(this Image image)
        {
            return ((MemoryStream)image.ToStream()).ToArray();
        }

        public static Stream ToStream(this Image image)
        {
            using MemoryStream memoryStream = new MemoryStream();
            image.Save(memoryStream, ImageFormat.Jpeg);

            return memoryStream;
        }
    }
}
