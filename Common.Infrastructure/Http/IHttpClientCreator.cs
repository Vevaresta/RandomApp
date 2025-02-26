namespace Common.Shared.Http
{
    public interface IHttpClientCreator
    {
        HttpClient GetHttpClient();
    }
}
