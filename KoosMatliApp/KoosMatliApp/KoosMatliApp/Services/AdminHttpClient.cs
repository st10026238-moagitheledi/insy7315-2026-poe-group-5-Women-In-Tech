namespace KoosMatliApp.Services;

public sealed class AdminHttpClient : HttpClient
{
    public AdminHttpClient(AdminSession session, AppConfiguration config)
        : base(new ApiRequestHandler(session) { InnerHandler = new HttpClientHandler() })
    {
        BaseAddress = config.ApiUri;
        Timeout = System.Threading.Timeout.InfiniteTimeSpan;
    }
}
