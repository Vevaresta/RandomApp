using NLog;

namespace RandomApp.Presentation.Api.Configuration
{
    public static class DependencyCfg
    {
        public static void RegisterBackendServices(this IServiceCollection services, IConfiguration configuration)
        {
           
            ProductManagement.Application.Configuration.DependencyCfg.RegisterApplicationServices(services);
            ProductManagement.Infrastructure.Configuration.DependencyCfg.RegisterInfrastructureServices(services);
            ProductManagement.Infrastructure.Configuration.DependencyCfg.RegisterDbContext(services, configuration);
            SharedKernel.Authentication.Infrastructure.Configuration.DependencyCfg.RegisterAuthDbContext(services, configuration);
            SharedKernel.Authentication.Infrastructure.Configuration.DependencyCfg.ConfigureIdentity(services);
            SharedKernel.Authentication.Infrastructure.Configuration.DependencyCfg.RegisterAuthServices(services);
            SharedKernel.Authentication.Application.Configuration.JwtConfiguration.ConfigureJWT(services, configuration);
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
