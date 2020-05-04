using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace DrugCheckingCrawler.ContentWriter
{
    public class ImageFileWriter
    {
        public void WriteFile(string path, byte[] imageContent)
        {
            var memoryStream = new MemoryStream(imageContent);
            var image = Image.FromStream(memoryStream);
            image.Save(Path.Combine(path, "Image.jpg"), ImageFormat.Jpeg);
        }
    }
}
