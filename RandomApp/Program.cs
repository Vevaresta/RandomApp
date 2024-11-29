using Microsoft.OpenApi.Models;
using NLog.Web;
using Random.App.ProductManagement.Infrastructure.Configuration;
using RandomApp.Server.Api.Middleware;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        builder.Host.UseNLog();

        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "RandomApp",
                Version = "v1",
                Description = "Testing my routes"
            });
        });

        builder.Services.RegisterServices();
        builder.Services.RegisterDbContext(builder.Configuration);


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API");
            });
        }
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}