using Common.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RandomApp.ShoppingCartManagement.Domain.RepositoryInterfaces;
using RandomApp.ShoppingCartManagement.Infrastructure.DataAccess;
using RandomApp.ShoppingCartManagement.Infrastructure.RepositoryImplementation;

namespace RandomApp.ShoppingCartManagement.Infrastructure.Configuration
{
    public static class DependencyCfg
    {
        public static void RegisterInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
            services.AddScoped<IUnitOfWork, ShoppingCartUnitOfWork>();

        }
        public static void RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ShoppingCartDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("RandomApp.ShoppingCart.Infrastructure")));
        }
    }
}
