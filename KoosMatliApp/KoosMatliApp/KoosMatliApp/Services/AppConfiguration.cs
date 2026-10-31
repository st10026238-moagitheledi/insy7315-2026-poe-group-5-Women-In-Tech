using KoosMatliApp.Localization;

namespace KoosMatliApp.Services;

public sealed class AppConfiguration
{
    public string ApiBaseUrl { get; init; }
        = ApiEndpoint.BaseUrl;

    public Uri ApiUri
    {
        get
        {
            if (!Uri.TryCreate(
                    ApiBaseUrl,
                    UriKind.Absolute,
                    out var uri) ||
                (uri.Scheme != "https" &&
                 !(uri.Scheme == "http" && (uri.IsLoopback || uri.Host == "10.0.2.2"))))
            {
                throw new InvalidOperationException(
                    L.T("Configure a valid HTTPS API address (or local development address)."));
            }

            return uri;
        }
    }
}