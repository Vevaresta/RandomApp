using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RandomApp.Server.Authentication.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace RandomApp.Server.Authentication.Configuration
{
    public static class DependencyCfg
    {
        public static void RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AuthDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("RandomApp.Server.Authentication")));
        }
    }
}
