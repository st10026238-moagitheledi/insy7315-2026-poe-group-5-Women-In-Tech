using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Storage;

namespace KoosMatliApp.Services;

public class ParentApiService
{
    private readonly HttpClient _httpClient;

    private const string TokenKey = "auth_token";

    private const string WindowsBaseUrl = "http://localhost:5000";
    private const string AndroidEmulatorBaseUrl = "http://10.0.2.2:5000";

    private static readonly JsonSerializerOptions JsonOptions =
        new(JsonSerializerDefaults.Web);

    public ParentApiService()
    {
        var baseUrl = ApiEndpoint.BaseUrl;

        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(baseUrl),
            Timeout = TimeSpan.FromSeconds(30)
        };
    }

    // =========================================
    // PARENT DASHBOARD
    // GET /api/parent/dashboard
    // =========================================

    public Task<JsonDocument> GetDashboardAsync()
    {
        return GetAsync("/api/parent/dashboard");
    }

    // =========================================
    // MY CHILDREN
    // GET /api/parent/children
    // =========================================

    public Task<JsonDocument> GetChildrenAsync()
    {
        return GetAsync("/api/parent/children");
    }

    // =========================================
    // SHARED AUTHENTICATED GET REQUEST
    // =========================================

    private async Task<JsonDocument> GetAsync(string endpoint)
    {
        string? token;

        try
        {
            token = await SecureStorage.Default.GetAsync(TokenKey);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(
                $"Secure storage error: {ex}");

            throw new InvalidOperationException(
                "Unable to access your saved login session. " +
                "Please sign in again.", ex);
        }

        if (string.IsNullOrWhiteSpace(token))
        {
            throw new UnauthorizedAccessException(
                "Your session has expired. Please log in again.");
        }

        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            endpoint);

        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        using var response = await SendRequestAsync(request);

        string responseBody =
            await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            string message = GetErrorMessage(
                responseBody,
                endpoint,
                (int)response.StatusCode);

            if (response.StatusCode ==
                System.Net.HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException(message);
            }

            if (response.StatusCode ==
                System.Net.HttpStatusCode.Forbidden)
            {
                throw new HttpRequestException(
                    $"You do not have permission to access this resource. {message}");
            }

            throw new HttpRequestException(message);
        }

        try
        {
            return JsonDocument.Parse(responseBody);
        }
        catch (JsonException ex)
        {
            System.Diagnostics.Debug.WriteLine(
                $"Invalid JSON from {endpoint}: {ex}");

            throw new HttpRequestException(
                "The server returned an invalid response. Please try again.",
                ex);
        }
    }

    // =========================================
    // HTTP REQUEST HANDLING
    // =========================================

    private async Task<HttpResponseMessage> SendRequestAsync(
        HttpRequestMessage request)
    {
        try
        {
            return await _httpClient.SendAsync(request);
        }
        catch (TaskCanceledException ex)
            when (!ex.CancellationToken.IsCancellationRequested)
        {
            throw new TimeoutException(
                "The request timed out. Please try again.",
                ex);
        }
        catch (HttpRequestException ex)
        {
            System.Diagnostics.Debug.WriteLine(
                $"Parent API connection error: {ex}");

            throw new HttpRequestException(
                "Unable to connect to the school server. " +
                "Check your connection and make sure the API is running.",
                ex);
        }
    }

    // =========================================
    // API ERROR MESSAGE EXTRACTION
    // =========================================

    private static string GetErrorMessage(
        string responseBody,
        string endpoint,
        int statusCode)
    {
        string message =
            $"Unable to load the requested information ({statusCode}).";

        try
        {
            using var errorJson = JsonDocument.Parse(responseBody);

            if (errorJson.RootElement.ValueKind !=
                JsonValueKind.Object)
            {
                return message;
            }

            if (TryGetString(
                    errorJson.RootElement,
                    out string? apiMessage,
                    "message",
                    "error",
                    "detail"))
            {
                message = apiMessage!;
            }
        }
        catch (JsonException)
        {
            // Keep the default error message.
        }

        System.Diagnostics.Debug.WriteLine(
            $"Parent API error: {endpoint} returned {statusCode}. {message}");

        return message;
    }

    private static bool TryGetString(
        JsonElement element,
        out string? value,
        params string[] propertyNames)
    {
        foreach (string propertyName in propertyNames)
        {
            if (element.TryGetProperty(
                    propertyName, out JsonElement property)
                && property.ValueKind == JsonValueKind.String)
            {
                value = property.GetString();
                return !string.IsNullOrWhiteSpace(value);
            }
        }

        value = null;
        return false;
    }

    public async Task<JsonDocument> GetAnnouncementsAsync()
    {
        var token = await SecureStorage.Default.GetAsync(TokenKey);

        if (string.IsNullOrWhiteSpace(token))
        {
            throw new UnauthorizedAccessException(
                "Your session has expired. Please log in again.");
        }

        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            "/api/parent/announcements");

        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        using var response = await _httpClient.SendAsync(request);

        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            string message =
                $"Unable to load announcements ({(int)response.StatusCode}).";

            try
            {
                using var errorJson = JsonDocument.Parse(responseBody);

                if (errorJson.RootElement.TryGetProperty(
                        "message", out var messageElement))
                {
                    message = messageElement.GetString() ?? message;
                }
            }
            catch (JsonException)
            {
                // Keep the default error message.
            }

            if (response.StatusCode ==
                System.Net.HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException(message);
            }

            throw new HttpRequestException(message);
        }

        return JsonDocument.Parse(responseBody);
    }

    // =========================================
    // LOG OUT
    // =========================================

    public void Logout()
    {
        SecureStorage.Default.Remove(TokenKey);
    }
}
