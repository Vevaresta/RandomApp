using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RandomApp.Server.Authentication.DataAccess;
using Microsoft.EntityFrameworkCore;
using RandomApp.Server.Authentication.Models;
using Microsoft.AspNetCore.Identity;
using RandomApp.Server.Authentication.Mapping;
using RandomApp.Server.Authentication.Services;
using Common.Shared.Authorization;

namespace RandomApp.Server.Authentication.Configuration
{
    public static class DependencyCfg
    {
        public static void RegisterAuthDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AuthDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("RandomApp.Server.Authentication")));
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(o =>
            {
                o.Password.RequireDigit = true;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 10;
                o.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();
        }

        public static void RegisterAuthServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AuthMappingProfile));
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.RequireAdminPolicy, policy =>
                    policy.RequireRole("Administrator"));

                options.AddPolicy(Policies.RequireUserPolicy, policy =>
                    policy.RequireRole("User"));
            });
        }


    }
}
