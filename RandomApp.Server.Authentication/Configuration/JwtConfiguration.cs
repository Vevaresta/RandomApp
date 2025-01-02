using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace RandomApp.Server.Authentication.Configuration
{
    public static class JwtConfiguration
    {
        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings.GetValue<string>("SecretKey");

            if (string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException("JWT Secret Key is not configured");
            }

            var logger = LogManager.GetCurrentClassLogger();
            logger.Info("Configuring JWT with Issuer: {0}, Audience: {1}",
                jwtSettings["validIssuer"],
                jwtSettings["validAudience"]);

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero,

                        ValidIssuer = jwtSettings["validIssuer"],
                        ValidAudience = jwtSettings["validAudience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                    };


                    // maybe remove after test
                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            logger.Info("Token successfully validated");
                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = context =>
                        {
                            logger.Error("Token validation failed: {0}", context.Exception.Message);
                            if (context.Exception is SecurityTokenExpiredException)
                            {
                                logger.Error("Token has expired");
                            }
                            else if (context.Exception is SecurityTokenInvalidAudienceException)
                            {
                                logger.Error("Invalid audience. Expected: {0}, Received: {1}",
                                    jwtSettings["validAudience"],
                                    context.Exception.Message);
                            }
                            else if (context.Exception is SecurityTokenInvalidIssuerException)
                            {
                                logger.Error("Invalid issuer. Expected: {0}, Received: {1}",
                                    jwtSettings["validIssuer"],
                                    context.Exception.Message);
                            }
                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            logger.Warn("Authentication challenge issued. Error: {0}", context.Error);
                            return Task.CompletedTask;
                        },
                        OnMessageReceived = context =>
                        {
                            var token = context.Token;
                            logger.Info("Received token: {0}", token?.Substring(0, Math.Min(50, token?.Length ?? 0)));
                            return Task.CompletedTask;
                        }
                    };
                });

        }
    }
}
