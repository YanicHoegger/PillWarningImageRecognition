using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace ManipulationClient
{
    public interface IExecuter
    {
        Task Execute(IConfiguration configuration, IServiceProvider serviceProvider);
    }
}
