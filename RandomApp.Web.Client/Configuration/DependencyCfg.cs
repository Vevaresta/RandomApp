using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RandomApp.ProductManagement.Application.Services;
using RandomApp.Web.Client.Services;

namespace RandomApp.Web.Client.Configuration
{
    public static class DependencyCfg
    {
        public static void RegisterFrontendServices(this IServiceCollection services)
        {
            services.AddSingleton<ProductSyncService>();
            services.AddSingleton<IHostedService>(sp => sp.GetRequiredService<ProductSyncService>());
            services.AddSingleton<IProductSyncService>(sp => sp.GetRequiredService<ProductSyncService>());

            services.AddSingleton<IHttpClientCreator, HttpClientCreator>();
            services.AddScoped<IProductService, ProductService>();
            services.AddHttpClient();
        }
    }
}
