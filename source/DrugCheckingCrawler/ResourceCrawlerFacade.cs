using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DrugCheckingCrawler
{
    public class ResourceCrawler
    {
        private const int MaxNoResult = 100;

        private readonly List<CrawlerResultItem> _resultItemList = new List<CrawlerResultItem>();
        private readonly ResourceDownloader resourceDownloader = new ResourceDownloader();
        private int noResultCount = 0;
        private int lastSuccessfullIndex = 0;

        public async Task<CrawlerResult> Crawl(int startIndex)
        {
            foreach (var (downloadTask, address, index) in resourceDownloader.GetPdfs(startIndex))
            {
                var downloadedContent = await downloadTask;

                var parser = new Parser();
                var parsed = parser.ParseFile(downloadedContent);

                if (parsed == null)
                {
                    noResultCount++;
                    if (noResultCount > MaxNoResult)
                    {
                        return new CrawlerResult(_resultItemList, lastSuccessfullIndex);
                    }
                    continue;
                }

                lastSuccessfullIndex = index;
                noResultCount = 0;
                _resultItemList.Add(new CrawlerResultItem(parsed, address, CreateHash(downloadedContent)));
            }

            throw new Exception("Program flow must have gone wrong");
        }

        private static string CreateHash(byte[] downloadedContent)
        {
            using SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            return Convert.ToBase64String(sha1.ComputeHash(downloadedContent));
        }
    }
}
