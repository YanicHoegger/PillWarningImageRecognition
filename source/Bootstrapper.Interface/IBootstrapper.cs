using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bootstrapper.Interface
{
    public interface IBootstrapper
    {
        void ConfigureServices(IServiceCollection services, IConfiguration configuration);
    }
}
