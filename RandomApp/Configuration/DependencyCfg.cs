using NLog;

namespace RandomApp.Server.Api.Configuration
{
    public static class DependencyCfg
    {
        public static void RegisterBackendServices(this IServiceCollection services)
        {
            ProductManagement.Infrastructure.Configuration.DependencyCfg.RegisterInfrastructureServices(services);
            ProductManagement.Application.Configuration.DependencyCfg.RegisterApplicationServices(services);
        }

        public static void RegisterLogging(this IServiceCollection services)
        {
            services.AddScoped<NLog.ILogger>(sp => LogManager.GetCurrentClassLogger());
        }
    }
}
