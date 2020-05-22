using NUnit.Framework;
using System.IO;
using Utilities;

namespace CustomVisionInteraction.UnitTests
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
