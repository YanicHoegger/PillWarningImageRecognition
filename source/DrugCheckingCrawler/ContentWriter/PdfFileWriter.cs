using System.IO;

namespace DrugCheckingCrawler
{
    public class PdfFileWriter
    {
        public void WriteFile(string path, byte[] fileContent)
        {
            File.WriteAllBytes(Path.Combine(path, "Content.pdf"), fileContent);
        }
    }
}
