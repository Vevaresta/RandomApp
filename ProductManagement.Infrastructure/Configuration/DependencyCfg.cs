using Common.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Random.App.ProductManagement.Domain.RepositoryInterfaces;
using Random.App.ProductManagement.Infrastructure.DataAccess;
using Random.App.ProductManagement.Infrastructure.RepositoryImplementation;


namespace Random.App.ProductManagement.Infrastructure.Configuration
{
    public static class DependencyCfg
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IUnitOfWork, ProductUnitOfWork>();
        }

        public static void RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ProductDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("ProductManagement.Infrastructure")));
        }
    }
}
