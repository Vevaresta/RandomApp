﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RandomApp.ProductManagement.Application.Services.Interfaces;
using RandomApp.Web.Client.Services;

namespace RandomApp.Web.Client.Configuration
{
    public static class DependencyCfg
    {
        public static void RegisterWebClientServices(this IServiceCollection services)
        {
            services.AddSingleton<ProductSyncService>();
            services.AddSingleton<IHostedService>(sp => sp.GetRequiredService<ProductSyncService>());
            services.AddSingleton<IProductSyncService>(sp => sp.GetRequiredService<ProductSyncService>());

            services.AddScoped<IProductService, ProductService>();
        }
    }
}
