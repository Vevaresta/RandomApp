using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using RandomApp.ProductManagement.Infrastructure.Configuration;
using RandomApp.Web.Client.Configuration;
using System.Text.Json.Serialization;
using RandomApp.Server.Authentication.Configuration;
using RandomApp.Presentation.Api.Configuration;
using RandomApp.Presentation.Api.Middleware;

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


            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
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
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {

                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }

                });
            });


            builder.Services.RegisterBackendServices(builder.Configuration);          
            builder.Services.RegisterLogging();

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

            // Authentication has to come before Authorization otherwise your tokens wont be validated because authorization
            // middleware tries to check permissions before the user is autenticated
            app.UseAuthentication();
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