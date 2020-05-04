using System.IO;
using System.Text.Json;

namespace DrugCheckingCrawler.ContentWriter
{
    public class InfoFileWriter
    {
        public void WriteInfo(string path, InfoFileContent infoFileContent)
        {
            var json = JsonSerializer.Serialize(infoFileContent);
            File.WriteAllText(Path.Combine(path, "Info.txt"), json);
        }
    }
}
