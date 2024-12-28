using Common.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RandomApp.ProductManagement.Domain.RepositoryInterfaces;
using RandomApp.ProductManagement.Infrastructure.DataAccess;
using RandomApp.ProductManagement.Infrastructure.RepositoryImplementation;


namespace RandomApp.ProductManagement.Infrastructure.Configuration
{
    public static class DependencyCfg
    {
        public static void RegisterInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IUnitOfWork, ProductUnitOfWork>();
            
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
