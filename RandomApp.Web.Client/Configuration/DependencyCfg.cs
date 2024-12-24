using Microsoft.Extensions.DependencyInjection;
using RandomApp.ProductManagement.Application.Services;
using RandomApp.Web.Client.Services;

namespace RandomApp.Web.Client.Configuration
{
    public static class DependencyCfg
    {
        public static void RegisterFrontendServices(this IServiceCollection services)
        {
            services.AddScoped<IHttpClientCreator, HttpClientCreator>();
            services.AddScoped<IProductService, ProductService>();
            services.AddHttpClient();
            services.AddHostedService<ProductSyncService>();
        }
    }
}
