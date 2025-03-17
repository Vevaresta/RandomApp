using Common.Shared.Http;
using RandomApp.ProductManagement.Application.Mapping;
using RandomApp.ProductManagement.Application.Services.Interfaces;
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
            //services.AddScoped<IProductRepository, ClientProductRepository>();
            ////services.AddScoped<IProductDisplayService, ProductDisplayService>();
            //services.AddScoped<IProductDisplayService>(serviceProvider => MockProductDisplayService.CreateMockProductService());
            services.AddAutoMapper(typeof(ProductMappingProfile).Assembly);

        }
    }
}
