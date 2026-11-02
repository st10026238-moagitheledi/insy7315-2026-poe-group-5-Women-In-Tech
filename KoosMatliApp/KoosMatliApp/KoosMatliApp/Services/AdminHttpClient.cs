namespace KoosMatliApp.Services;

public sealed class AdminHttpClient : HttpClient
{
    public AdminHttpClient(AdminSession session, AppConfiguration config) //(Microsoft Learn, 2026c).
        : base(new ApiRequestHandler(session) { InnerHandler = new HttpClientHandler() }) //(Microsoft Learn, 2026a).
    {
        BaseAddress = config.ApiUri;
        Timeout = System.Threading.Timeout.InfiniteTimeSpan; //(Microsoft Learn, 2026b).
    }
}

/*
 Reference List

Microsoft Corporation, "Make HTTP Requests Using IHttpClientFactory in .NET: Message Handlers," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/core/extensions/httpclient-factory#message-handlers

Microsoft Corporation, "HttpClient.Timeout Property (System.Net.Http)," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpclient.timeout

Microsoft Corporation, "Configuration in .NET," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/core/extensions/configuration
 */