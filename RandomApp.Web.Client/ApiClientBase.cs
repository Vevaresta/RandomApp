namespace RandomApp.Web.Client
{
    public abstract class ApiClientBase
    {
        private IHttpClientCreator _httpClientCreator;

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
