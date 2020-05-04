using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace DrugCheckingCrawler
{
    public class ResourceDownloader
    {
        private const int MaxNumber = 2000;

        public IEnumerable<(Task<byte[]> downloadTask, string address)> GetPdfs()
        {
            for(var i = 1; i <= MaxNumber; i++)
            {
                var address = @$"https://de.drugchecking.ch/pdf.php?p={i}";
                using var client = new WebClient();
                yield return (client.DownloadDataTaskAsync(address), address);
            }
        }
    }
}
