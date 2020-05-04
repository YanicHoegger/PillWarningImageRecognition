using DrugCheckingCrawler.ContentWriter;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DrugCheckingCrawler
{
    public class ResourceCreatorFacade
    {
        private readonly string _resourceDestination;

        public ResourceCreatorFacade(string resourceDestination)
        {
            _resourceDestination = resourceDestination;
        }

        public async Task CreateResources()
        {
            var resourceDownloader = new ResourceDownloader();

            foreach (var (downloadTask, address) in resourceDownloader.GetPdfs())
            {
                await CreateResource(downloadTask, address);
            }
        }

        private async Task CreateResource(Task<byte[]> downloadTask, string address)
        {
            Console.WriteLine($"Download from '{address}'");

            byte[] downloadedContent;
            try
            {
                downloadedContent = await downloadTask;
            }
            catch (WebException)
            {
                Console.WriteLine($"Could not download from {address}");
                return;
            }

            var parser = new Parser();
            var parsed = parser.ParseFile(downloadedContent);

            if (parsed == null)
                return;

            WriteContent(address, downloadedContent, parsed);
        }

        private void WriteContent(string address, byte[] downloadedContent, ParserResult parsed)
        {
            string id = CreateId(downloadedContent);
            var path = Path.Combine(_resourceDestination, $"{parsed.Name} ({id})");

            Console.WriteLine($"Write to  '{path}'");

            Directory.CreateDirectory(path);

            var infoWriter = new InfoFileWriter();
            infoWriter.WriteInfo(path, new InfoFileContent(parsed, address, id));

            var imageFileWriter = new ImageFileWriter();
            imageFileWriter.WriteFile(path, parsed.Image);

            var pdfFileWriter = new PdfFileWriter();
            pdfFileWriter.WriteFile(path, downloadedContent);
        }

        private static string CreateId(byte[] downloadedContent)
        {
            string id;
            using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider())
            {
                id = Convert.ToBase64String(sha1.ComputeHash(downloadedContent));
                //Replace slashs otherwise it would create a new folder as part of a path
                id = id.Replace('\\', '_');
                id = id.Replace('/', '.');
            }

            return id;
        }
    }
}
