using Common.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductManagement.Domain.RepositoryInterfaces;
using ProductManagement.Infrastructure.ORM;
using ProductManagement.Infrastructure.RepositoryImplementation;


namespace ProductManagement.Infrastructure.Config
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ProductDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("ProductManagement.Infrastructure")));

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IUnitOfWork, ProductUnitOfWork>();

            return services;
        }
    }
}
