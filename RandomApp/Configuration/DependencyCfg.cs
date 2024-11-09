using System.Reflection;

namespace RandomApp.Server.Api.Configuration
{
    public static class DependencyCfg
    {
        public static void RegisterServices(this IServiceCollection services, Assembly startupProjectAssembly, IConfiguration configuration)
        {
            Random.App.ProductManagement.Infrastructure.Configuration.DependencyCfg.RegisterServices(services);
        }
    }
}
