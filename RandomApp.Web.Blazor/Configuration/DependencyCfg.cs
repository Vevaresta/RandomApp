using NLog;
using RandomApp.ProductManagement.Application.Mapping;
using RandomApp.ProductManagement.Domain.RepositoryInterfaces;
using RandomApp.Web.Client;
using RandomApp.Web.Client.Services;


namespace RandomApp.Web.Blazor.Configuration
{
    public static class DependencyCfg
    {
        public static void RegisterBlazorServices(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddSingleton<IHttpClientCreator, HttpClientCreator>();
            services.AddScoped<IProductRepository, ClientProductRepository>();
            services.AddScoped<IProductDisplayService, ProductDisplayService>();
            services.AddAutoMapper(typeof(ProductMappingProfile).Assembly);
            
        }

        public static void RegisterLogging(this IServiceCollection services)
        {
            services.AddScoped<NLog.ILogger>(sp => LogManager.GetCurrentClassLogger());
        }
    }
}
