using Common.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RandomApp.ProductManagement.Application.Services.Interfaces;
using RandomApp.ProductManagement.Infrastructure.DataAccess;
using RandomApp.ProductManagement.Infrastructure.RepositoryImplementation;
using RandomApp.ProductManagement.Infrastructure.Services.ExternalApi;


namespace RandomApp.ProductManagement.Infrastructure.Configuration
{
    public static class DependencyCfg
    {
        public static void RegisterInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IUnitOfWork, ProductUnitOfWork>();
            services.AddSingleton<ProductSyncService>();
            services.AddSingleton<IHostedService>(sp => sp.GetRequiredService<ProductSyncService>());
            services.AddSingleton<IProductSyncService>(sp => sp.GetRequiredService<ProductSyncService>());

            services.AddScoped<IProductService, ProductService>();

        }

        public static void RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ProductDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("RandomApp.ProductManagement.Infrastructure")));
        }
    }
}
