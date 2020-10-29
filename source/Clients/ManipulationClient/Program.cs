using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ManipulationClient
{
    class Program
    {
        /// <summary>
        /// Use this application for manipulations, such as updating databases etc.
        /// </summary>
        static void Main(string[] args)
        {
            try
            {
                Startup.Init(args);

                var config = Startup.ServiceProvider.GetService<IConfiguration>();
                var executer = Startup.ServiceProvider.GetServices<IExecuter>();

                Task.WaitAll(executer.Select(x => x.Execute(config, Startup.ServiceProvider)).ToArray());
            }
            catch (AggregateException aggregateException)
            {
                foreach (var innerException in aggregateException.InnerExceptions)
                {
                    Console.WriteLine(@$"Exception occurred: {innerException.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(@$"Exception occurred: {ex.Message}");
            }
        }
    }
}
