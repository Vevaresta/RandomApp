namespace RandomApp.Server.Api.Configuration
{
    public static class DependencyCfg
    {
        public static void RegisterBackendServices(this IServiceCollection services)
        {
            ProductManagement.Infrastructure.Configuration.DependencyCfg.RegisterBackendServices(services);           
        }
    }
}
