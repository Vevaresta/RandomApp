namespace RandomApp.Server.Api.Configuration
{
    public static class DependencyCfg
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            Random.App.ProductManagement.Infrastructure.Configuration.DependencyCfg.RegisterServices(services);           
        }
    }
}
