﻿using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using RandomApp.ProductManagement.Application.DataTransferObjects;
using RandomApp.ProductManagement.Application.Mapping;
using RandomApp.ProductManagement.Application.Orchestrators;

namespace RandomApp.ProductManagement.Application.Configuration
{
    public static class DependencyCfg
    {
        public static void RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IProductSyncOrchestrator, ProductSyncOrchestrator>();
            services.AddAutoMapper(typeof(ProductMappingProfile));
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<ProductDtoValidator>();
        }
    }
}
