using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;

namespace DrugCheckingCrawler
{
    internal class Program
    {
        internal static void Main()
        {
            var parser = new Parser();
            var parserResult = parser.ParseFile(File.ReadAllBytes(@"C:\Users\Yanic\Desktop\Temp\doc.pdf"));

            var memoryStream = new MemoryStream(parserResult.Image.First());
            var image = Image.FromStream(memoryStream);
            image.Save(@"C:\Users\Yanic\Desktop\Temp\74.jpg", ImageFormat.Jpeg);
        }
    }
}
