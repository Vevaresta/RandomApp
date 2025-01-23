using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RandomApp.ShoppingCart.Infrastructure.DataAccess;

namespace RandomApp.ShoppingCart.Infrastructure.Configuration
{
    public static class DependencyCfg
    {
        public static void RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ShoppingCartDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("RandomApp.ShoppingCart.Infrastructure")));
        }
    }
}
