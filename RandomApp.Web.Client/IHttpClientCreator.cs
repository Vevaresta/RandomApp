namespace RandomApp.Web.Client
{
    public interface IHttpClientCreator
    {
        HttpClient GetHttpClient();

        Task RefreshClientAsync();
    }
}
