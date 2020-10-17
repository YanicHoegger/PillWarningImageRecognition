using DrugCheckingCrawler.Interface;
using DrugCheckingCrawler.Parsers;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DrugCheckingCrawler
{
    public class ResourceCrawler : IResourceCrawler
    {
        private const int _maxNoResult = 100;

        private readonly ResourceDownloader _resourceDownloader = new ResourceDownloader();
        private readonly Parser _parser;
        private readonly ILogger<ResourceCrawler> _resourceLogger;

        public ResourceCrawler(Parser parser, ILogger<ResourceCrawler> resourceLogger)
        {
            _parser = parser;
            _resourceLogger = resourceLogger;
        }

        public ICrawlerResult Crawl(int startIndex)
        {
            var lastSuccessfulIndexTask = new TaskCompletionSource<int>();
            return new CrawlerResult(CrawlAsync(startIndex, lastSuccessfulIndexTask), lastSuccessfulIndexTask.Task);
        }

        private async IAsyncEnumerable<ICrawlerResultItem> CrawlAsync(int startIndex, TaskCompletionSource<int> lastSuccessfulIndexTask)
        {
            int noResultCount = 0;
            int lastSuccessfulIndex = 0;

            foreach ((Task<byte[]> downloadTask, string address, int index) in _resourceDownloader.GetPdfs(startIndex))
            {
                _resourceLogger.LogInformation($"Try download Nr.: {index}");
                var downloadedContent = await downloadTask;

                var parsed = _parser.ParseFile(downloadedContent);

                if (parsed == null)
                {
                    noResultCount++;
                    if (noResultCount > _maxNoResult)
                    {
                        lastSuccessfulIndexTask.SetResult(lastSuccessfulIndex);
                        yield break;
                    }
                    continue;
                }

                lastSuccessfulIndex = index;
                noResultCount = 0;

                yield return new CrawlerResultItem(parsed, address, CreateHash(downloadedContent));
            }

            throw new InvalidOperationException("Program flow must have gone wrong");
        }

        private static string CreateHash(byte[] downloadedContent)
        {
            using SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
            return Convert.ToBase64String(sha1.ComputeHash(downloadedContent));
        }
    }
}
