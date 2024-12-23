namespace RandomApp.Web.Client
{
    // Provides a base class that any API client (like Product Service) can inherit
    public abstract class ApiClientBase
    {
        private readonly IHttpClientCreator _httpClientCreator;

        protected ApiClientBase(IHttpClientCreator httpClientCreator)
        {
            _httpClientCreator = httpClientCreator;
        }

        public HttpClient HttpClient
        {
            get
            {
                return _httpClientCreator.GetHttpClient();
            }
        }
    }
}
