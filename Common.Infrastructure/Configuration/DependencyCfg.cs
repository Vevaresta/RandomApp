using Common.Shared.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Shared.Configuration
{
    public static class DependencyCfg
    {
        public static void HttpClientServices(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddSingleton<IHttpClientCreator, HttpClientCreator>();

        }
    }
}
