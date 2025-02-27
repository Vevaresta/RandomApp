﻿using NLog;

namespace RandomApp.Presentation.Api.Configuration
{
    public static class DependencyCfg
    {
        public static void RegisterBackendServices(this IServiceCollection services, IConfiguration configuration)
        {
           
            ProductManagement.Application.Configuration.DependencyCfg.RegisterApplicationServices(services);
            ProductManagement.Infrastructure.Configuration.DependencyCfg.RegisterInfrastructureServices(services);
            ProductManagement.Infrastructure.Configuration.DependencyCfg.RegisterDbContext(services, configuration);
            Authentication.Configuration.DependencyCfg.RegisterAuthDbContext(services, configuration);
            Authentication.Configuration.DependencyCfg.ConfigureIdentity(services);
            Authentication.Configuration.DependencyCfg.RegisterAuthServices(services);
            Authentication.Configuration.JwtConfiguration.ConfigureJWT(services, configuration);
            ShoppingCartManagement.Application.Configuration.DependencyCfg.RegisterApplicationServices(services);
            ShoppingCartManagement.Infrastructure.Configuration.DependencyCfg.RegisterInfrastructureServices(services);
            ShoppingCartManagement.Infrastructure.Configuration.DependencyCfg.RegisterDbContext(services, configuration);
            Common.Shared.Configuration.DependencyCfg.HttpClientServices(services);

        }

        public static void RegisterLogging(this IServiceCollection services)
        {
            services.AddScoped<NLog.ILogger>(sp => LogManager.GetCurrentClassLogger());
        }
    }
}
