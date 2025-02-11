using Microsoft.Extensions.DependencyInjection;
using RandomApp.ShoppingCartManagement.Application.Controllers;
using RandomApp.ShoppingCartManagement.Application.Mapping;
using RandomApp.ShoppingCartManagement.Application.Services;
using RandomApp.ShoppingCartManagement.Domain.RepositoryInterfaces;


namespace RandomApp.ShoppingCartManagement.Application.Configuration
{

    public static class DependencyCfg
    {
        public static void RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ShoppingCartMappingProfile));
            services.AddControllers()
                    .AddApplicationPart(typeof(ShoppingCartController).Assembly);
        }

    }

}
