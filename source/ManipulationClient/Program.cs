using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

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
                var executer = Startup.ServiceProvider.GetService<IExecuter>();

                executer.Execute(config, Startup.ServiceProvider).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occoured: {ex.Message}");
            }
        }
    }
}
