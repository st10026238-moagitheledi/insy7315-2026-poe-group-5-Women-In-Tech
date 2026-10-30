using KoosMatliApp.Localization;

namespace KoosMatliApp.Services;

public sealed class AppConfiguration
{
    public string ApiBaseUrl { get; init; }
        = "https://localhost:52386/";

    public Uri ApiUri
    {
        get
        {
            if (!Uri.TryCreate(
                    ApiBaseUrl,
                    UriKind.Absolute,
                    out var uri) ||
                uri.Scheme != "https")
            {
                throw new InvalidOperationException(
                    L.T("Configure a valid HTTPS API address."));
            }

            return uri;
        }
    }
}