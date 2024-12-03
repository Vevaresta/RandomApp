using Microsoft.Extensions.Configuration;

namespace RandomApp.Web.Client
{
    public class HttpClientCreator : IHttpClientCreator
    {
        private IHttpClientFactory _httpClientFactory;
        //private ILocalStorageService _localStorage;
        private IConfiguration _configuration;

        public HttpClient GetHttpClient()
        {
            var httpClient = _httpClientFactory.CreateClient("RandomApp_ApiHttpClient");
            httpClient.BaseAddress = new Uri(_configuration.GetValue<string>("baseAddress") ?? "");
            return httpClient;
        }

        public Task RefreshClientAsync()
        {
            throw new NotImplementedException();
        }
    }
}
