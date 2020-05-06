using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace CustomVisionInteraction.UnitTests
{
    //TODO: create heler or utilities assembly and move it there
    public static class ImageHelper
    {
        public static byte[] ReadImage(string path)
        {
            var image = Image.FromFile(path);

            using MemoryStream memoryStream = new MemoryStream();
            image.Save(memoryStream, ImageFormat.Jpeg);

            return memoryStream.ToArray();
        }
    }
}
