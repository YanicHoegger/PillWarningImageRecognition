using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace DrugCheckingCrawler
{
    public class ResourceDownloader
    {
        public IEnumerable<(Task<byte[]> downloadTask, string address, int index)> GetPdfs(int startIndex)
        {
            for (var i = startIndex; true; i++)
            {
                var address = @$"https://de.drugchecking.ch/pdf.php?p={i}";
                using var client = new WebClient();
                yield return (client.DownloadDataTaskAsync(address), address, i);
            }
        }
    }
}
