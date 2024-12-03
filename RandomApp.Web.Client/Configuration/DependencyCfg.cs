using Microsoft.Extensions.DependencyInjection;

namespace RandomApp.Web.Client.Configuration
{
    public static class DependencyCfg
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<IHttpClientCreator, HttpClientCreator>();
            //services.AddTransient<IProductService, ProductService>();
            //services.AddTransient<IProductSyncService, ProductSyncService>();
        }
    }
}
