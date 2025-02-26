using Microsoft.Extensions.Configuration;


namespace Common.Shared.Http
{
    // centralizes http client creation
    public class HttpClientCreator : IHttpClientCreator
    {
        private readonly IHttpClientFactory _httpClientFactory;
        //private ILocalStorageService _localStorage; maybe later
        private readonly IConfiguration _configuration;


        public HttpClientCreator(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public HttpClient GetHttpClient()
        {

            var httpClient = _httpClientFactory.CreateClient();
            var baseAddress = _configuration.GetValue<string>("ApiSettings:BaseAddress");

            if (string.IsNullOrWhiteSpace(baseAddress))
            {
                throw new InvalidOperationException("Base address is not configured.");
            }
            httpClient.BaseAddress = new Uri(baseAddress);

            return httpClient;
        }
    }
}
