using NLog;

namespace RandomApp.Server.Api.Configuration
{
    public static class DependencyCfg
    {
        public static void RegisterBackendServices(this IServiceCollection services, IConfiguration configuration)
        {
            Web.Client.Configuration.DependencyCfg.RegisterWebClientServices(services);
            ProductManagement.Application.Configuration.DependencyCfg.RegisterApplicationServices(services);
            ProductManagement.Infrastructure.Configuration.DependencyCfg.RegisterInfrastructureServices(services);
            ProductManagement.Infrastructure.Configuration.DependencyCfg.RegisterDbContext(services, configuration);
            Authentication.Configuration.DependencyCfg.RegisterAuthDbContext(services, configuration);
            Authentication.Configuration.DependencyCfg.ConfigureIdentity(services);
            Authentication.Configuration.DependencyCfg.RegisterAuthServices(services);
            Authentication.Configuration.JwtConfiguration.ConfigureJWT(services, configuration);

        }

        public static void RegisterLogging(this IServiceCollection services)
        {
            services.AddScoped<NLog.ILogger>(sp => LogManager.GetCurrentClassLogger());
        }
    }
}
