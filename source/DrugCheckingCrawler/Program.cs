using System;
using System.IO;

namespace DrugCheckingCrawler
{
    internal class Program
    {
        internal static void Main()
        {
            var basePath = Environment.CurrentDirectory.Replace(@"source\DrugCheckingCrawler\bin\Debug\netcoreapp3.1", "");
            var path = Path.Combine(basePath, "DrugCheckingResources");

            Console.WriteLine("Start collecting resources");

            var resourceCreatorFacade = new ResourceCreatorFacade(path);
            try
            {
                resourceCreatorFacade.CreateResources().Wait();

                Console.WriteLine("Finished collecting resources");
            }
            catch (AggregateException aggregateException)
            {
                foreach (var exception in aggregateException.InnerExceptions)
                {
                    Console.Error.WriteLine(exception.Message);
                    Console.Error.WriteLine(exception.StackTrace);
                }
            }
        }
    }
}
