using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blazor.FileReader;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace WebInterface.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddFileReaderService(options => options.UseWasmSharedBuffer = true);

            await builder.Build().RunAsync();
        }
    }
}
