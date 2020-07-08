using NUnit.Framework;
using System.IO;

namespace DrugCheckingCrawler.UnitTests
{
    public static class TestHelper
    {
        public static string GetAbsolutePath(string fileName)
        {
            return Path.Combine(TestContext.CurrentContext.WorkDirectory, "TestFiles", fileName);
        }
    }
}
