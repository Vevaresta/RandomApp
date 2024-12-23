using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using RandomApp.ProductManagement.Infrastructure.Configuration;
using RandomApp.Server.Api.Middleware;
using RandomApp.Web.Client.Configuration;

internal class Program
{
    private static void Main(string[] args)
    {
        var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
        try
        {
            // TO DO: "Starting application" is not showing in log
            logger.Info("Starting application...");
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();
            builder.Host.UseNLog();

            builder.Services.AddControllers();
            // if my API calls often send the same data, instead of having to to back to the DB, with this methods I can cache the data in memory
            builder.Services.AddResponseCaching();

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

            builder.Services.RegisterFrontendServices();
            builder.Services.RegisterBackendServices();
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
        catch (Exception ex)
        {
            logger.Fatal(ex, "Application startup failed.");
            throw; //Re-throw the exception to ensure the application exits
        }
        finally
        {
            NLog.LogManager.Shutdown();
        }


    }
}