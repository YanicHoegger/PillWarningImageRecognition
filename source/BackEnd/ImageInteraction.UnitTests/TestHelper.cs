using System.IO;
using NUnit.Framework;
using Utilities;

namespace ImageInteraction.UnitTests
{
    public static class TestHelper
    {
        public static byte[] ReadImage(string fileName)
        {
            return ImageFileHelper.ReadImageAsArray(GetTestFilePath(fileName));
        }

        public static string GetTestFilePath(string fileName)
        {
            return Path.Combine(TestContext.CurrentContext.WorkDirectory, "TestFiles", fileName);
        }
    }
}
