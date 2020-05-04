using System.Collections.Generic;
using System.Net;

namespace DrugCheckingCrawler
{
    public class PdfCollector
    {
        private const int MaxNumber = 10000;

        public IEnumerable<byte[]> GetPdfs()
        {
            for(var i = 1; i <= MaxNumber; i++)
            {
                using var client = new WebClient();
                yield return client.DownloadData(@$"https://de.drugchecking.ch/pdf.php?p={i}");
            }
        }
    }
}
