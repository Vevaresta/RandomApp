using Microsoft.Extensions.Configuration;
using NLog;

namespace RandomApp.Web.Client
{
    // centralizes http client creation
    public class HttpClientCreator : IHttpClientCreator
    {
        private readonly IHttpClientFactory _httpClientFactory;
        //private ILocalStorageService _localStorage;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public HttpClientCreator(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            this._logger = LogManager.GetCurrentClassLogger();
            _logger.Info("HttpClientCreator service initialized");
        }

        public HttpClient GetHttpClient()
        {
            _logger.Info("Creating new HTTP client for API communication.");

            var httpClient = _httpClientFactory.CreateClient("RandomApp_ApiHttpClient");
            var baseAddress = _configuration.GetValue<string>("ApiSettings:BaseAddress");

            _logger.Debug("Configuring client with base address: {BaseAddress}",
                baseAddress?.TrimEnd('/') ?? "null");

            if (string.IsNullOrWhiteSpace(baseAddress))
            {
                _logger.Error("Base adress configuration is missing or empty.");
                throw new InvalidOperationException("Base address is not configured.");
            }
            httpClient.BaseAddress = new Uri(baseAddress);

            _logger.Info("HTTP client successfully configured and ready for use.");

            return httpClient;
        }

        public Task RefreshClientAsync()
        {
            throw new NotImplementedException();
        }
    }
}
