using Microsoft.Extensions.DependencyInjection;
using RandomApp.Web.Client.Products;

namespace RandomApp.Web.Client.Configuration
{
    public static class DependencyCfg
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<IHttpClientCreator, HttpClientCreator>();
            services.AddTransient<IProductService, ProductService>();
            services.AddHttpClient();
            //services.AddTransient<IProductSyncService, ProductSyncService>();
        }
    }
}
