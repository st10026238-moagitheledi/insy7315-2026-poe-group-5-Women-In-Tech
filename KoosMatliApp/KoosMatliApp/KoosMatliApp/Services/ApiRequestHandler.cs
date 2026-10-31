using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace KoosMatliApp.Services;

public sealed class ApiRequestHandler(AdminSession session)
    : DelegatingHandler
{
    public static TimeSpan RequestTimeout { get; set; } =
        TimeSpan.FromSeconds(90);

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        using var budget =
            CancellationTokenSource.CreateLinkedTokenSource(
                cancellationToken);

        budget.CancelAfter(RequestTimeout);

        var version = session.Version;

        try
        {
            var token = await session.GetIdTokenAsync(budget.Token);

            if (string.IsNullOrWhiteSpace(token))
            {
                return Error(
                    request,
                    HttpStatusCode.Unauthorized,
                    L.T("Connect the signed-in user before loading admin data."));
            }

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            request.Headers.Accept.ParseAdd("application/json");
            request.Headers.AcceptLanguage.Clear();
            request.Headers.AcceptLanguage.ParseAdd(L.Culture.Name);

            var response = await base.SendAsync(request, budget.Token);

            try
            {
                await response.Content.LoadIntoBufferAsync(budget.Token);
            }
            catch
            {
                response.Dispose();
                throw;
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                session.NotifyAuthorizationRequired(version);

            return response;
        }
        catch (OperationCanceledException)
            when (!cancellationToken.IsCancellationRequested)
        {
            return Error(
                request,
                HttpStatusCode.GatewayTimeout,
                AdminText.T(
                    "The server took too long. Check that the API is running, then try again."));
        }
        catch (HttpRequestException)
        {
            return Error(
                request,
                HttpStatusCode.ServiceUnavailable,
                L.T("The server could not complete the request."));
        }
    }

    private static HttpResponseMessage Error(
        HttpRequestMessage request,
        HttpStatusCode status,
        string message) =>
        new(status)
        {
            RequestMessage = request,
            Content = JsonContent.Create(new { message })
        };
}