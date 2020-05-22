using DrugCheckingCrawler.Interface;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DrugCheckingCrawler
{
    public class ResourceCrawler : IResourceCrawler
    {
        private const int _maxNoResult = 100;

        private readonly List<CrawlerResultItem> _resultItemList = new List<CrawlerResultItem>();
        private readonly ResourceDownloader _resourceDownloader = new ResourceDownloader();
        private readonly Parser _parser;
        private int _noResultCount = 0;
        private int _lastSuccessfullIndex = 0;

        public ResourceCrawler(Parser parser)
        {
            _parser = parser;
        }

        public async Task<ICrawlerResult> Crawl(int startIndex)
        {
            foreach (var (downloadTask, address, index) in _resourceDownloader.GetPdfs(startIndex))
            {
                var downloadedContent = await downloadTask;

                var parsed = _parser.ParseFile(downloadedContent);

                if (parsed == null)
                {
                    _noResultCount++;
                    if (_noResultCount > _maxNoResult)
                    {
                        return new CrawlerResult(_resultItemList, _lastSuccessfullIndex);
                    }
                    continue;
                }

                _lastSuccessfullIndex = index;
                _noResultCount = 0;
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
