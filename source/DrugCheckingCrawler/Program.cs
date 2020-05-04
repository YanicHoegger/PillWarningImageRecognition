using System;

namespace DrugCheckingCrawler
{
    internal class Program
    {
        internal static void Main()
        {

            var collector = new PdfCollector();

            var index = 1;
            var writer = new PdfFileWriter(@"C:\Users\Yanic\Desktop\Temp");
            foreach(var file in collector.GetPdfs())
            {
                writer.WriteFile(index++.ToString(), file);
            }

            Console.WriteLine("Hello World!");

        }
    }
}
