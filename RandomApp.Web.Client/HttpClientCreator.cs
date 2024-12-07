using Microsoft.Extensions.Configuration;

namespace RandomApp.Web.Client
{
    // centralizes http client creation
    public class HttpClientCreator : IHttpClientCreator
    {
        private IHttpClientFactory _httpClientFactory;
        //private ILocalStorageService _localStorage;
        private IConfiguration _configuration;

        public HttpClientCreator(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public HttpClient GetHttpClient()
        {
            //var httpClient = _httpClientFactory.CreateClient("RandomApp_ApiHttpClient");
            //httpClient.BaseAddress = new Uri(_configuration.GetValue<string>("baseAddress") ?? "");
            //return httpClient;
            var httpClient = _httpClientFactory.CreateClient("RandomApp_ApiHttpClient");
            var baseAddress = _configuration.GetValue<string>("ApiSettings:BaseAddress");
            if (string.IsNullOrWhiteSpace(baseAddress))
            {
                throw new InvalidOperationException("Base address is not configured.");
            }
            httpClient.BaseAddress = new Uri(baseAddress);
            return httpClient;
        }

        public Task RefreshClientAsync()
        {
            throw new NotImplementedException();
        }
    }
}
