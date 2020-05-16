﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebInterface.Server
{
    public static class PillWarningDomainExtension
    {
        public static void AddDomain(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            new Bootstrapper.Bootstrapper().ConfigureServices(serviceCollection, configuration);
        }
    }
}
