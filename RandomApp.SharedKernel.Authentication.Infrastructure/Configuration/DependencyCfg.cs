using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Common.Shared.Authorization;
using RandomApp.SharedKernel.Authentication.Infrastructure.Persistence;
using RandomApp.SharedKernel.Authentication.Domain.Models;
using RandomApp.SharedKernel.Authentication.Application.Mapping;
using RandomApp.SharedKernel.Authentication.Application.Services;
using RandomApp.SharedKernel.Authentication.Application.Interfaces;

namespace RandomApp.SharedKernel.Authentication.Infrastructure.Configuration
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
