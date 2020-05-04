using System.IO;

namespace DrugCheckingCrawler
{
    public class PdfFileWriter
    {
        private readonly string _baseAddress;

        public PdfFileWriter(string baseAddress)
        {
            _baseAddress = baseAddress;
        }

        public void WriteFile(string name, byte[] fileContent)
        {
            File.WriteAllBytes(Path.Combine(_baseAddress, $"{name}.pdf"), fileContent);
        }
    }
}
